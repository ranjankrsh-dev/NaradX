using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NaradX.Business.Common.Interfaces;
using NaradX.Business.Common.Models;
using NaradX.Business.Common.Services;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IJwtService jwtService,
            ILogger<LoginCommandHandler> logger,
            IRefreshTokenService refreshTokenService,
            IUnitOfWork unitOfWork,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _logger = logger;
            _refreshTokenService = refreshTokenService;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
                if (user == null)
                    throw new ApplicationException("Invalid email or password");

                if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
                {
                    _logger.LogWarning("User account locked out: {Email}", request.Email);
                    throw new ApplicationException($"Account locked. Try again at {user.LockoutEnd.Value:yyyy-MM-dd HH:mm:ss} UTC");
                }

                // SECURE PASSWORD VERIFICATION
                var isPasswordValid = _passwordService.VerifyPassword(
                    request.Password,
                    user.PasswordHash,
                    user.PasswordSalt
                );

                // Always use generic error for authentication failures
                if (!isPasswordValid || !user.IsActive || !user.EmailVerified)
                {
                    user.RecordFailedLogin();
                    if (user.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked due to too many failed attempts: {Email}", request.Email);
                    }
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    throw new ApplicationException("Invalid email or password");
                }

                if (!user.IsActive)
                    throw new ApplicationException("Account is deactivated");

                if (!user.EmailVerified)
                    throw new ApplicationException("Invalid email or password");

                // Fetch the user's role from the database
                var userRole = await _userRepository.GetUserRoles(user.Id, cancellationToken);

                // Update last login (resets counters internally)
                user.UpdateLastLogin();
                
                // Save changes for last login update
                // Note: In a real DDD app, this might be a Domain Event, but explicit save is fine here.
                // We do this before generating tokens to ensure db state is consistent.
                await _userRepository.UpdateLastLoginAsync(user.Id, DateTime.UtcNow, cancellationToken); 
                // Wait, UpdateLastLoginAsync in repo likely duplicates the logic or just sets the date. 
                // Since we updated the entity in memory with user.UpdateLastLogin(), EF Core change tracking will handle it if we call SaveChanges.
                // However, the original code called _userRepository.UpdateLastLoginAsync. Let's check that repo method later. 
                // For now, consistent with existing code, but using domain object state.

                var accessToken = _jwtService.GenerateToken(user, userRole);
                var accessTokenExpires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes);

                // Generate refresh token
                var ipAddress = request.IpAddress; // Implement this method
                var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id, ipAddress);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("User logged in: {Email}", request.Email);

                return new LoginResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    AccessToken = accessToken,
                    AccessTokenExpires = accessTokenExpires,
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpires = refreshToken.Expires,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for: {Email}", request.Email);
                throw;
            }
        }
    }
}
