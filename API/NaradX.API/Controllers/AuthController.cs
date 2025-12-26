// <copyright file="AuthController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.API.Controllers;

using System.Security.Claims;
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

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator,
                      IMemoryCache memoryCache,
                      ILogger<AuthController> logger) : ControllerBase
{
    private const int RATELIMITATTEMPTS = 10;
    private static readonly TimeSpan RATELIMITWINDOW = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan RATELIMITBLOCK = TimeSpan.FromMinutes(15);
    private readonly IMediator mediator = mediator;
    private readonly IMemoryCache memoryCache = memoryCache;
    private readonly ILogger<AuthController> logger = logger;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
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
        if (memoryCache.TryGetValue(blockKey, out _))
        {
            logger.LogWarning("Login attempt BLOCKED: Email={Email}, IP={IP}, Time={Time}", email, ipAddress, timestamp);
            return StatusCode(429, new { Message = "Too many login attempts. Please try again later." });
        }

        // Increment attempt count
        int attempts = memoryCache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = RATELIMITWINDOW;
            return 0;
        });
        attempts++;
        memoryCache.Set(cacheKey, attempts, RATELIMITWINDOW);

        if (attempts > RATELIMITATTEMPTS)
        {
            // Block this IP
            logger.LogWarning("Login attempt RATE LIMITED: Email={Email}, IP={IP}, Time={Time}", email, ipAddress, timestamp);
            memoryCache.Set(blockKey, true, RATELIMITBLOCK);
            return StatusCode(429, new { Message = "Too many login attempts. Please try again later." });
        }

        try
        {
            command.IpAddress = ipAddress;
            var result = await mediator.Send(command);
            // Reset attempts on successful login
            memoryCache.Remove(cacheKey);
            logger.LogInformation("Login SUCCESS: Email={Email}, IP={IP}, Time={Time}", email, ipAddress, timestamp);
            return Ok(result);
        }
        catch (ApplicationException ex)
        {
            logger.LogWarning("Login FAILURE: Email={Email}, IP={IP}, Time={Time}, Reason={Reason}", email, ipAddress, timestamp, ex.Message);
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Login ERROR: Email={Email}, IP={IP}, Time={Time}", email, ipAddress, timestamp);
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

            var result = await mediator.Send(command);
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

            var result = await mediator.Send(command);
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
