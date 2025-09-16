using Microsoft.EntityFrameworkCore;
using NaradX.Domain.Entities.Auth;
using NaradX.Domain.Enums;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly NaradXDbContext _appDbContext;

        public RoleRepository(NaradXDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        }

        public async Task<Role?> GetByTypeAsync(RoleType roleType, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == roleType.ToString(), cancellationToken);
        }

        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Roles
                .AnyAsync(r => r.Name == name, cancellationToken);
        }

        public async Task<bool> IsSystemRoleAsync(int roleId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Roles
                .AnyAsync(r => r.Id == roleId && r.IsSystemRole, cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Roles
                .Where(r => r.IsSystemRole)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetAssignableRolesAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Roles
                .Where(r => !r.IsSystemRole) // Only non-system roles can be assigned
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId, CancellationToken cancellationToken = default)
        {
            // Check if user already has this role
            var existingAssignment = await _appDbContext.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);

            if (existingAssignment != null)
                return true; // Already assigned

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                CreatedOn = DateTime.UtcNow
            };

            await _appDbContext.UserRoles.AddAsync(userRole, cancellationToken);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId, CancellationToken cancellationToken = default)
        {
            var userRole = await _appDbContext.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);

            if (userRole == null)
                return false; // Role not assigned to user

            _appDbContext.UserRoles.Remove(userRole);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> UserHasRoleAsync(int userId, string roleName, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.UserRoles
                .Include(ur => ur.Role)
                .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == roleName, cancellationToken);
        }

        public async Task<bool> UserHasRoleTypeAsync(int userId, RoleType roleType, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.UserRoles
                .Include(ur => ur.Role)
                .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == roleType.ToString(), cancellationToken);
        }

        public async Task<int> RemoveAllRolesFromUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            var userRoles = await _appDbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync(cancellationToken);

            _appDbContext.UserRoles.RemoveRange(userRoles);
            return await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UpdateRoleAsync(Role role, CancellationToken cancellationToken = default)
        {
            _appDbContext.Roles.Update(role);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IEnumerable<User>> GetUsersInRoleAsync(int roleId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.UserRoles
                .Where(ur => ur.RoleId == roleId)
                .Include(ur => ur.User)
                .ThenInclude(u => u.Tenant)
                .Select(ur => ur.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.UserRoles
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                .ThenInclude(u => u.Tenant)
                .Where(ur => ur.Role.Name == roleName)
                .Select(ur => ur.User)
                .ToListAsync(cancellationToken);
        }
    }
}
