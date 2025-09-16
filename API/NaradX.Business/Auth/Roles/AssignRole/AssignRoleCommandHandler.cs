using MediatR;
using Microsoft.Extensions.Logging;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.Roles.AssignRole
{
    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, AssignRoleResponse>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignRoleCommandHandler> _logger;

        public AssignRoleCommandHandler(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<AssignRoleCommandHandler> logger)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<AssignRoleResponse> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user exists
                var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
                if (user == null)
                    return new AssignRoleResponse { Success = false, Message = "User not found" };

                // Check if role exists
                var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
                if (role == null)
                    return new AssignRoleResponse { Success = false, Message = "Role not found" };

                // Check if user already has this role
                var userRoles = await _roleRepository.GetUserRolesAsync(request.UserId, cancellationToken);
                if (userRoles.Any(r => r.Id == request.RoleId))
                    return new AssignRoleResponse { Success = false, Message = "User already has this role" };

                // Assign role to user
                var success = await _roleRepository.AssignRoleToUserAsync(request.UserId, request.RoleId, cancellationToken);
                if (!success)
                    return new AssignRoleResponse { Success = false, Message = "Failed to assign role" };

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Role {RoleId} assigned to user {UserId} by {AssignedBy}",
                    request.RoleId, request.UserId, request.AssignedBy);

                return new AssignRoleResponse
                {
                    Success = true,
                    Message = $"Role '{role.Name}' assigned to user successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role {RoleId} to user {UserId}",
                    request.RoleId, request.UserId);
                return new AssignRoleResponse { Success = false, Message = "Error assigning role" };
            }
        }
    }
}
