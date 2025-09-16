using MediatR;
using Microsoft.Extensions.Logging;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;
        private readonly IRefreshTokenService _refreshTokenService;

        public ChangePasswordCommandHandler(
            IUserRepository userRepository,
            IPasswordService passwordService,
            ILogger<ChangePasswordCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get user from database
                var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
                if (user == null)
                    throw new ApplicationException("User not found");

                // Verify current password
                var isCurrentPasswordValid = _passwordService.VerifyPassword(
                    request.CurrentPassword,
                    user.PasswordHash,
                    user.PasswordSalt
                );

                if (!isCurrentPasswordValid)
                    throw new ApplicationException("Current password is incorrect");

                // Create new password hash
                (var newPasswordHash, var newPasswordSalt) = _passwordService.CreateHash(request.NewPassword);

                // Update user password
                user.PasswordHash = newPasswordHash;
                user.PasswordSalt = newPasswordSalt;
                user.UpdatedOn = DateTime.UtcNow;

                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                // Revoke all refresh tokens for this user after password change
                await _refreshTokenService.RevokeAllRefreshTokensForUserAsync(user.Id, "PasswordChanged");

                _logger.LogInformation("Password changed successfully for user ID: {UserId}", request.UserId);

                return new ChangePasswordResponse
                {
                    Message = "Password changed successfully",
                    ChangedAt = DateTime.UtcNow
                };
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning("Password change failed for user ID: {UserId} - {Message}",
                    request.UserId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user ID: {UserId}", request.UserId);
                throw new ApplicationException("Error changing password");
            }
        }
    }
}
