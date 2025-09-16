using MediatR;
using Microsoft.Extensions.Logging;
using NaradX.Business.Auth.Login;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(
            IRefreshTokenService refreshTokenService,
            IJwtService jwtService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<RefreshTokenCommandHandler> logger)
        {
            _refreshTokenService = refreshTokenService;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate refresh token
                var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);
                if (refreshToken == null || !refreshToken.IsActive)
                    throw new ApplicationException("Invalid refresh token");

                // Get user
                var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);
                if (user == null || !user.IsActive)
                    throw new ApplicationException("User not found or inactive");

                // Revoke current refresh token
                await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken, request.IpAddress);

                var userRole = await _userRepository.GetUserRoles(user.Id, cancellationToken);

                // Generate new tokens
                var accessToken = _jwtService.GenerateToken(user, userRole);
                var accessTokenExpires = DateTime.UtcNow.AddMinutes(15);
                var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id, request.IpAddress);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Token refreshed for user ID: {UserId}", user.Id);

                return new LoginResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    AccessToken = accessToken,
                    AccessTokenExpires = accessTokenExpires,
                    RefreshToken = newRefreshToken.Token,
                    RefreshTokenExpires = newRefreshToken.Expires,
                    Message = "Token refreshed successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                throw;
            }
        }
    }
}
