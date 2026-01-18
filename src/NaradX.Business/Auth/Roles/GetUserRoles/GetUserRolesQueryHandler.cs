using MediatR;
using Microsoft.Extensions.Logging;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.Roles.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, GetUserRolesResponse>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUserRolesQueryHandler> _logger;

        public GetUserRolesQueryHandler(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            ILogger<GetUserRolesQueryHandler> logger)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<GetUserRolesResponse> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user exists
                var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
                if (user == null)
                    throw new ApplicationException("User not found");

                // Get user roles
                var roles = await _roleRepository.GetUserRolesAsync(request.UserId, cancellationToken);

                return new GetUserRolesResponse
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    Roles = roles.Select(r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description ?? string.Empty,
                        IsSystemRole = r.IsSystemRole
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles for user {UserId}", request.UserId);
                throw;
            }
        }
    }
}
