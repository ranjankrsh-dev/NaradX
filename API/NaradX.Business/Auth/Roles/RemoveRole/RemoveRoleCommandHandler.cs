using MediatR;
using Microsoft.Extensions.Logging;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.Roles.RemoveRole
{
    public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, RemoveRoleResponse>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveRoleCommandHandler> _logger;

        public RemoveRoleCommandHandler(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<RemoveRoleCommandHandler> logger)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<RemoveRoleResponse> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user exists
                var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
                if (user == null)
                    return new RemoveRoleResponse { Success = false, Message = "User not found" };

                // Check if role exists
                var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
                if (role == null)
                    return new RemoveRoleResponse { Success = false, Message = "Role not found" };

                // Check if user has this role
                var userRoles = await _roleRepository.GetUserRolesAsync(request.UserId, cancellationToken);
                if (!userRoles.Any(r => r.Id == request.RoleId))
                    return new RemoveRoleResponse { Success = false, Message = "User does not have this role" };

                // Prevent removal of last admin role if needed
                if (role.IsSystemRole && userRoles.Count(r => r.IsSystemRole) <= 1)
                    return new RemoveRoleResponse { Success = false, Message = "Cannot remove the last system role from user" };

                // Remove role from user
                var success = await _roleRepository.RemoveRoleFromUserAsync(request.UserId, request.RoleId, cancellationToken);
                if (!success)
                    return new RemoveRoleResponse { Success = false, Message = "Failed to remove role" };

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Role {RoleId} removed from user {UserId} by {RemovedBy}",
                    request.RoleId, request.UserId, request.RemovedBy);

                return new RemoveRoleResponse
                {
                    Success = true,
                    Message = $"Role '{role.Name}' removed from user successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing role {RoleId} from user {UserId}",
                    request.RoleId, request.UserId);
                return new RemoveRoleResponse { Success = false, Message = "Error removing role" };
            }
        }
    }
}
