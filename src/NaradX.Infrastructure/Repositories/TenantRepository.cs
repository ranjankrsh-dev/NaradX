using Microsoft.EntityFrameworkCore;
using NaradX.Domain.Entities.Tenancy;
using NaradX.Domain.Enums;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Infrastructure.Repositories
{
    public class TenantRepository : Repository<Tenant>, ITenantRepository
    {
        private readonly NaradXDbContext _appDbContext;

        public TenantRepository(NaradXDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task<Tenant?> GetBySubdomainAsync(string subdomain, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Tenants
                .FirstOrDefaultAsync(t => t.Domain == subdomain, cancellationToken);
        }

        public async Task<Tenant?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Tenants
                .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
        }

        public async Task<bool> SubdomainExistsAsync(string subdomain, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Tenants
                .AnyAsync(t => t.Domain == subdomain, cancellationToken);
        }

        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Tenants
                .AnyAsync(t => t.Name == name, cancellationToken);
        }

        //public async Task<IEnumerable<Tenant>> GetTenantsByStatusAsync(TenantStatus status, CancellationToken cancellationToken = default)
        //{
        //    return await _appDbContext.Tenants
        //        .Where(t => t.Status == status)
        //        .ToListAsync(cancellationToken);
        //}

        public async Task<IEnumerable<Tenant>> GetTenantsWithUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Tenants
                .Include(t => t.Users)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ActivateTenantAsync(int tenantId, string activatedBy, CancellationToken cancellationToken = default)
        {
            var tenant = await _appDbContext.Tenants.FindAsync(new object[] { tenantId }, cancellationToken);
            if (tenant == null) return false;

            tenant.IsActive = true;
            tenant.IsActive = true;
            tenant.DeactivatedOn = null;
            tenant.DeactivatedBy = null;
            tenant.UpdatedOn = DateTime.UtcNow;
            tenant.UpdatedBy = activatedBy;

            _appDbContext.Tenants.Update(tenant);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeactivateTenantAsync(int tenantId, string deactivatedBy, CancellationToken cancellationToken = default)
        {
            var tenant = await _appDbContext.Tenants.FindAsync(new object[] { tenantId }, cancellationToken);
            if (tenant == null) return false;

            tenant.IsActive = false;
            tenant.DeactivatedOn = DateTime.UtcNow;
            tenant.DeactivatedBy = deactivatedBy;
            tenant.UpdatedOn = DateTime.UtcNow;
            tenant.UpdatedBy = deactivatedBy;

            _appDbContext.Tenants.Update(tenant);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> SuspendTenantAsync(int tenantId, string suspendedBy, CancellationToken cancellationToken = default)
        {
            var tenant = await _appDbContext.Tenants.FindAsync(new object[] { tenantId }, cancellationToken);
            if (tenant == null) return false;

            tenant.IsActive = false;
            tenant.DeactivatedOn = DateTime.UtcNow;
            tenant.DeactivatedBy = suspendedBy;
            tenant.UpdatedOn = DateTime.UtcNow;
            tenant.UpdatedBy = suspendedBy;

            _appDbContext.Tenants.Update(tenant);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<int> GetUsersCountAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Users
                .CountAsync(u => u.TenantId == tenantId && !u.IsDeleted, cancellationToken);
        }

        public async Task<int> GetActiveTenantsCountAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Tenants
                .CountAsync(t => t.IsActive && !t.IsDeleted, cancellationToken);
        }

        public async Task<bool> UpdateSubscriptionAsync(int tenantId, DateTime endDate, int maxUsers, int maxMessages, CancellationToken cancellationToken = default)
        {
            var tenant = await _appDbContext.Tenants.FindAsync(new object[] { tenantId }, cancellationToken);
            if (tenant == null) return false;

            tenant.SubscriptionEndDate = endDate;
            tenant.MaxUsers = maxUsers;
            tenant.MaxMessagesPerMonth = maxMessages;
            tenant.UpdatedOn = DateTime.UtcNow;

            _appDbContext.Tenants.Update(tenant);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> CheckSubscriptionValidityAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            var tenant = await _appDbContext.Tenants.FindAsync(new object[] { tenantId }, cancellationToken);
            if (tenant == null) return false;

            // Check if subscription is active and not expired
            var isSubscriptionValid = tenant.SubscriptionEndDate.HasValue &&
                                    tenant.SubscriptionEndDate.Value >= DateTime.UtcNow;

            // Update status if needed
            //if (!isSubscriptionValid && tenant.Status == TenantStatus.Active)
            //{
            //    tenant.Status = TenantStatus.Expired;
            //    tenant.IsActive = false;
            //    tenant.UpdatedAt = DateTime.UtcNow;

            //    _appDbContext.Tenants.Update(tenant);
            //    await _appDbContext.SaveChangesAsync(cancellationToken);
            //}

            return isSubscriptionValid;
        }

        public async Task<int> GetTotalTenantsCountAsync(CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Tenants
                .CountAsync(t => !t.IsDeleted, cancellationToken);
        }

        public async Task<decimal> GetMonthlyRevenueAsync(CancellationToken cancellationToken = default)
        {
            // This would typically involve a more complex calculation
            // based on your billing system
            var activeTenants = await _appDbContext.Tenants
                .Where(t => t.IsActive && !t.IsDeleted)
                .CountAsync(cancellationToken);

            // Assuming each tenant pays $99/month
            return activeTenants * 99m;
        }

        public async Task<IEnumerable<Tenant>> GetTenantsWithExpiringSubscriptionsAsync(int daysThreshold, CancellationToken cancellationToken = default)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);

            return await _appDbContext.Tenants
                .Where(t => t.SubscriptionEndDate.HasValue &&
                           t.SubscriptionEndDate.Value <= thresholdDate &&
                           t.SubscriptionEndDate.Value >= DateTime.UtcNow &&
                           t.IsActive &&
                           !t.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> RenewSubscriptionAsync(int tenantId, int months, CancellationToken cancellationToken = default)
        {
            var tenant = await _appDbContext.Tenants.FindAsync(new object[] { tenantId }, cancellationToken);
            if (tenant == null) return false;

            // Set new subscription end date
            var newEndDate = tenant.SubscriptionEndDate.HasValue &&
                            tenant.SubscriptionEndDate.Value > DateTime.UtcNow
                ? tenant.SubscriptionEndDate.Value.AddMonths(months)
                : DateTime.UtcNow.AddMonths(months);

            tenant.SubscriptionEndDate = newEndDate;
            tenant.IsActive = true;
            tenant.UpdatedOn = DateTime.UtcNow;

            _appDbContext.Tenants.Update(tenant);
            return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
