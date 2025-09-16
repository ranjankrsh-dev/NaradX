using MediatR;
using Microsoft.Extensions.Logging;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Entities.Auth;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IUnitOfWork unitOfWork,
            ILogger<RegisterCommandHandler> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.Email) && await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
                    throw new ApplicationException("Email already exists");

                if (!string.IsNullOrEmpty(request.PhoneNumber) &&
                    await _userRepository.PhoneExistsAsync(request.PhoneNumber, cancellationToken))
                {
                    throw new ApplicationException("Phone number is already registered.");
                }

                // SECURE PASSWORD HASHING
                (var passwordHash, var passwordSalt) = _passwordService.CreateHash(request.Password);

                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt, // Store salt
                    TenantId = request.TenantId,
                    IsActive = true,
                    EmailVerified = false,
                    CreatedOn = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("User registered: {Email}", request.Email);

                return new RegisterResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user: {Email}", request.Email);
                throw;
            }
        }
    }
}
