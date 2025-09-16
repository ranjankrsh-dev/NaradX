using NaradX.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default);

        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> PhoneExistsAsync(string phoneNumber, CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetActiveUsersAsync(int tenantId, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetUsersByTenantAsync(int tenantId, CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetUsersWithTenantAsync(CancellationToken cancellationToken = default);
        Task<User?> GetUserWithTenantAsync(int userId, CancellationToken cancellationToken = default);

        Task<bool> DeactivateUserAsync(int userId, string deactivatedBy, CancellationToken cancellationToken = default);
        Task<bool> ReactivateUserAsync(int userId, string reactivatedBy, CancellationToken cancellationToken = default);

        Task UpdateLastLoginAsync(int userId, DateTime loginDate, CancellationToken cancellationToken = default);
        Task<int> GetActiveUsersCountAsync(int tenantId, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserRole>> GetUserRoles(int userId, CancellationToken cancellationToken = default);
    }
}
