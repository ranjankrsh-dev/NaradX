using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaradX.Entities.Response.Auth;
using NaradX.Web.Security.Interfaces;
using NaradX.Web.ViewModels.Auth;
using System.Security.Claims;

namespace NaradX.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiHelper _apiHelper;
        private readonly ITokenService _tokenService;
        private readonly IAuditService _auditService;
        private readonly IConfiguration _configuration;
        private readonly IIpAddressService _ipAddressService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IApiHelper apiHelper,
            ITokenService tokenService,
            IAuditService auditService,
            IConfiguration configuration,
            ILogger<AuthController> logger,
            IIpAddressService ipAddressService)
        {
            _apiHelper = apiHelper;
            _tokenService = tokenService;
            _auditService = auditService;
            _configuration = configuration;
            _logger = logger;
            _ipAddressService = ipAddressService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            SetNoCacheHeaders();

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Default", "User");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            SetNoCacheHeaders();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var apiRequest = new
                {
                    email = model.Email,
                    password = model.Password,
                    ipAddress = _ipAddressService.GetClientIpAddress()
                };

                string apiURL = "api/Auth/login";
                var response = await _apiHelper.PostData<object, LoginResponse>(apiURL, apiRequest);

                if (response == null || string.IsNullOrEmpty(response.AccessToken))
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }

                // Validate the JWT token
                var principal = _tokenService.ValidateJwtToken(response.AccessToken);
                if (principal == null)
                {
                    ModelState.AddModelError("", "Invalid token received.");
                    return View(model);
                }

                // Extract claims from JWT
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.UserId.ToString()),
                    new Claim(ClaimTypes.Email, response.Email),
                    new Claim(ClaimTypes.Name, $"{principal.FindFirst(ClaimTypes.GivenName)?.Value} {principal.FindFirst(ClaimTypes.Surname)?.Value}"),
                    new Claim(ClaimTypes.Role, principal.FindFirst(ClaimTypes.Role)?.Value ?? "User"),
                    new Claim("TenantId", principal.FindFirst("tenantId")?.Value ?? "1"),
                    new Claim("JwtToken", response.AccessToken)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = response.AccessTokenExpires,
                    AllowRefresh = true,
                    IssuedUtc = DateTime.UtcNow
                };

                // Store tokens in session
                HttpContext.Session.SetString("authToken", response.AccessToken);
                HttpContext.Session.SetString("refreshToken", response.RefreshToken);
                HttpContext.Session.SetString("tokenExpiry", response.AccessTokenExpires.ToString("O"));
                HttpContext.Session.SetString("userEmail", response.Email);
                HttpContext.Session.SetInt32("userId", response.UserId);

                // Sign in
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Log successful login
                await _auditService.LogLoginAsync(response.UserId, HttpContext.Connection.RemoteIpAddress?.ToString());

                return RedirectToLocal(model.ReturnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user {Email}", model.Email);
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.Session.GetInt32("userId");

            // Clear session
            HttpContext.Session.Clear();

            // Sign out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear authentication cookie
            Response.Cookies.Delete("NaradX");

            SetNoCacheHeaders();

            if (userId.HasValue)
            {
                await _auditService.LogLogoutAsync(userId.Value);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckSession()
        {
            var authToken = HttpContext.Session.GetString("authToken");
            if (string.IsNullOrEmpty(authToken) || _tokenService.IsTokenExpired(authToken))
            {
                await HttpContext.SignOutAsync();
                HttpContext.Session.Clear();
                return Json(new { isAuthenticated = false });
            }

            return Json(new { isAuthenticated = true });
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            SetNoCacheHeaders();
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Default", "User");
        }

        private void SetNoCacheHeaders()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
        }
    }
}
