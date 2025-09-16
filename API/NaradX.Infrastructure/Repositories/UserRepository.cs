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
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly NaradXDbContext _appDbContext;

        public UserRepository(NaradXDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> PhoneExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .AnyAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
        }

        // REMOVED: GetUsersByStatusAsync since User doesn't have Status property
        // Use GetUsersByActiveStatusAsync instead (see bonus methods below)

        public async Task<IEnumerable<User>> GetUsersByTenantAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .Where(u => u.TenantId == tenantId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersWithTenantAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .Include(u => u.Tenant)
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetUserWithTenantAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<bool> DeactivateUserAsync(int userId, string deactivatedBy, CancellationToken cancellationToken = default)
        {
            var user = await _appDbContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user == null) return false;

            user.IsActive = false;
            user.DeactivatedOn = DateTime.UtcNow;
            user.DeactivatedBy = deactivatedBy;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = deactivatedBy;

            _appDbContext.Users.Update(user);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> ReactivateUserAsync(int userId, string reactivatedBy, CancellationToken cancellationToken = default)
        {
            var user = await _appDbContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user == null) return false;

            user.IsActive = true;
            user.DeactivatedOn = null;
            user.DeactivatedBy = null;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = reactivatedBy;

            _appDbContext.Users.Update(user);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task UpdateLastLoginAsync(int userId, DateTime loginDate, CancellationToken cancellationToken = default)
        {
            var user = await _appDbContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user != null)
            {
                user.LastLoginDate = loginDate;
                _appDbContext.Users.Update(user);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> GetActiveUsersCountAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .CountAsync(u => u.TenantId == tenantId &&
                               u.IsActive &&
                               !u.IsDeleted,
                           cancellationToken);
        }

        // Bonus methods with proper implementation

        public async Task<IEnumerable<User>> GetActiveUsersAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .Where(u => u.TenantId == tenantId && u.IsActive && !u.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetInactiveUsersAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .Where(u => u.TenantId == tenantId && !u.IsActive && !u.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetUserWithRolesAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User?> GetUserWithRolesAndTenantAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .Include(u => u.Tenant)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<bool> UpdateUserPasswordAsync(int userId, byte[] passwordHash, byte[] passwordSalt, CancellationToken cancellationToken = default)
        {
            var user = await _appDbContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user == null) return false;

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UpdatedOn = DateTime.UtcNow;

            _appDbContext.Users.Update(user);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> MarkEmailAsVerifiedAsync(int userId, CancellationToken cancellationToken = default)
        {
            var user = await _appDbContext.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user == null) return false;

            user.EmailVerified = true;
            user.EmailVerifiedAt = DateTime.UtcNow;
            user.UpdatedOn = DateTime.UtcNow;

            _appDbContext.Users.Update(user);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<int> GetTotalUsersCountAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .CountAsync(u => u.TenantId == tenantId && !u.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetRecentlyActiveUsersAsync(int tenantId, int days, CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);

            return await _appDbContext.Users
                .Where(u => u.TenantId == tenantId &&
                           u.LastLoginDate.HasValue &&
                           u.LastLoginDate.Value >= cutoffDate &&
                           u.IsActive &&
                           !u.IsDeleted)
                .OrderByDescending(u => u.LastLoginDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsUserOverLimitAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            var tenant = await _appDbContext.Tenants.FindAsync(new object[] { tenantId }, cancellationToken);
            if (tenant == null) return true;

            var activeUserCount = await GetActiveUsersCountAsync(tenantId, cancellationToken);
            return activeUserCount >= tenant.MaxUsers;
        }

        public async Task<IEnumerable<UserRole>> GetUserRoles(int userId, CancellationToken cancellationToken = default)
        {

            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();

            return userRoles;
        }
    }
}
