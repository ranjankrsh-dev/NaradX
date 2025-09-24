using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using NaradX.Entities.Request;
using NaradX.Entities.Response.Auth;
using NaradX.Web.Services.Interfaces.Common;
using NaradX.Web.Services.Interfaces.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NaradX.Web.Services.Implementations.Security
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiHelper _apiHelper;
        private readonly IConfiguration _configuration;

        public TokenService(IHttpContextAccessor httpContextAccessor, IApiHelper apiHelper, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _apiHelper = apiHelper;
            _configuration = configuration;
        }

        public async Task ValidateTokenAsync(CookieValidatePrincipalContext context)
        {
            var authToken = context.HttpContext.Session.GetString("authToken");
            var refreshToken = context.HttpContext.Session.GetString("refreshToken");
            var email = context.HttpContext.Session.GetString("userEmail");

            if (string.IsNullOrEmpty(authToken) || IsTokenExpired(authToken))
            {
                if (!string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(email))
                {
                    try
                    {
                        var newToken = await RefreshAccessTokenAsync(refreshToken);
                        if (!string.IsNullOrEmpty(newToken))
                        {
                            context.HttpContext.Session.SetString("authToken", newToken);

                            // Update the principal with new token claims
                            var newPrincipal = ValidateJwtToken(newToken);
                            context.ReplacePrincipal(newPrincipal);
                            context.ShouldRenew = true;
                            return;
                        }
                    }
                    catch
                    {
                        // Refresh failed
                    }
                }

                // No valid token, reject
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync();
                context.HttpContext.Session.Clear();
            }
        }

        public async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            try
            {
                var email = _httpContextAccessor.HttpContext.Session.GetString("userEmail");
                var request = new RefreshTokenRequest
                {
                    RefreshToken = refreshToken,
                    Email = email
                };

                var response = await _apiHelper.PostData<RefreshTokenRequest, LoginResponse>(
                    "api/refresh-token",
                    request
                );

                if (response != null && !string.IsNullOrEmpty(response.AccessToken))
                {
                    // Update session with new tokens
                    _httpContextAccessor.HttpContext.Session.SetString("authToken", response.AccessToken);
                    _httpContextAccessor.HttpContext.Session.SetString("refreshToken", response.RefreshToken);
                    _httpContextAccessor.HttpContext.Session.SetString("tokenExpiry", response.AccessTokenExpires.ToString());

                    return response.AccessToken;
                }
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Token refresh failed: {ex.Message}");
            }

            return null;
        }

        public bool IsTokenExpired(string token)
        {
            try
            {
                var expiry = GetTokenExpiry(token);
                return expiry <= DateTime.UtcNow.AddMinutes(1); // 1 minute buffer
            }
            catch
            {
                return true;
            }
        }

        public DateTime GetTokenExpiry(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.ValidTo;
        }

        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "NaradX.API",
                    ValidateAudience = true,
                    ValidAudience = "NaradX.Clients",
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    SaveSigninToken = false,
                    ValidateTokenReplay = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
