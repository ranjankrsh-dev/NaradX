using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NaradX.Business.Auth.ChangePassword;
using NaradX.Business.Auth.Login;
using NaradX.Business.Auth.RefreshToken;
using NaradX.Business.Auth.Register;
using System.Security.Claims;

namespace NaradX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<AuthController> _logger;
        private const int RATE_LIMIT_ATTEMPTS = 10;
        private static readonly TimeSpan RATE_LIMIT_WINDOW = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan RATE_LIMIT_BLOCK = TimeSpan.FromMinutes(15);

        public AuthController(IMediator mediator, IMemoryCache memoryCache, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string cacheKey = $"login_attempts_{ipAddress}";
            string blockKey = $"login_block_{ipAddress}";
            var email = command.Email;
            var timestamp = DateTime.UtcNow;

            // Check if IP is blocked
            if (_memoryCache.TryGetValue(blockKey, out _))
            {
                _logger.LogWarning("Login attempt BLOCKED: Email={Email}, IP={IP}, Time={Time}", email, ipAddress, timestamp);
                return StatusCode(429, new { Message = "Too many login attempts. Please try again later." });
            }

            // Increment attempt count
            int attempts = _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = RATE_LIMIT_WINDOW;
                return 0;
            });
            attempts++;
            _memoryCache.Set(cacheKey, attempts, RATE_LIMIT_WINDOW);

            if (attempts > RATE_LIMIT_ATTEMPTS)
            {
                // Block this IP
                _logger.LogWarning("Login attempt RATE LIMITED: Email={Email}, IP={IP}, Time={Time}", email, ipAddress, timestamp);
                _memoryCache.Set(blockKey, true, RATE_LIMIT_BLOCK);
                return StatusCode(429, new { Message = "Too many login attempts. Please try again later." });
            }

            try
            {
                command.IpAddress = ipAddress;
                var result = await _mediator.Send(command);
                // Reset attempts on successful login
                _memoryCache.Remove(cacheKey);
                _logger.LogInformation("Login SUCCESS: Email={Email}, IP={IP}, Time={Time}", email, ipAddress, timestamp);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning("Login FAILURE: Email={Email}, IP={IP}, Time={Time}, Reason={Reason}", email, ipAddress, timestamp, ex.Message);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login ERROR: Email={Email}, IP={IP}, Time={Time}", email, ipAddress, timestamp);
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                // Get user ID from token
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var command = new ChangePasswordCommand
                {
                    UserId = userId,
                    CurrentPassword = request.CurrentPassword,
                    NewPassword = request.NewPassword
                };

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error changing password" });
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

                var command = new RefreshTokenCommand
                {
                    RefreshToken = request.RefreshToken,
                    IpAddress = ipAddress
                };

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error refreshing token" });
            }
        }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
