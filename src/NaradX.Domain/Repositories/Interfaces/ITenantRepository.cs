using NaradX.Domain.Entities.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Repositories.Interfaces
{
    public interface ITenantRepository : IRepository<Tenant>
    {
        Task<Tenant?> GetBySubdomainAsync(string subdomain, CancellationToken cancellationToken = default);
        Task<Tenant?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<bool> SubdomainExistsAsync(string subdomain, CancellationToken cancellationToken = default);
        Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);

        //Task<IEnumerable<Tenant>> GetTenantsByStatusAsync(TenantStatus status, CancellationToken cancellationToken = default);
        Task<IEnumerable<Tenant>> GetTenantsWithUsersAsync(CancellationToken cancellationToken = default);

        Task<bool> ActivateTenantAsync(int tenantId, string activatedBy, CancellationToken cancellationToken = default);
        Task<bool> DeactivateTenantAsync(int tenantId, string deactivatedBy, CancellationToken cancellationToken = default);
        Task<bool> SuspendTenantAsync(int tenantId, string suspendedBy, CancellationToken cancellationToken = default);

        Task<int> GetUsersCountAsync(int tenantId, CancellationToken cancellationToken = default);
        Task<int> GetActiveTenantsCountAsync(CancellationToken cancellationToken = default);

        Task<bool> UpdateSubscriptionAsync(int tenantId, DateTime endDate, int maxUsers, int maxMessages, CancellationToken cancellationToken = default);
        Task<bool> CheckSubscriptionValidityAsync(int tenantId, CancellationToken cancellationToken = default);
    }
}
