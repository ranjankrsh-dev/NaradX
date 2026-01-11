using Microsoft.Extensions.Logging;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Entities.Tenancy;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Services
{
    public class TenantService : ITenantService
    {
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ILogger<TenantService> _logger;
        private int? _tenantId;
        private string? _tenantName;

        public TenantService(
            IRepository<Tenant> tenantRepository,
            ILogger<TenantService> logger)
        {
            _tenantRepository = tenantRepository;
            _logger = logger;
        }

        public int? TenantId => _tenantId;
        public string? TenantName => _tenantName;

        public async Task<bool> SetTenantFromClaimsAsync(ClaimsPrincipal user)
        {
            try
            {
                // Get tenant ID from claims (from JWT token)
                var tenantIdClaim = user.FindFirst("tenantId") ??
                                  user.FindFirst(ClaimTypes.GroupSid) ??
                                  user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/tenantId");

                if (tenantIdClaim != null && int.TryParse(tenantIdClaim.Value, out var tenantId))
                {
                    _tenantId = tenantId;

                    // Optionally load tenant name
                    var tenant = await _tenantRepository.GetByIdAsync(tenantId);
                    _tenantName = tenant?.Name;

                    _logger.LogInformation("Tenant set to: {TenantId} - {TenantName}", _tenantId, _tenantName);
                    return true;
                }

                _logger.LogWarning("Tenant ID not found in user claims");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting tenant from claims");
                return false;
            }
        }

        public async Task<bool> ValidateUserTenantAccessAsync(int userId, int tenantId)
        {
            // Implement if you need to validate user access to specific tenant
            // This is useful for admin users who might access multiple tenants
            return await Task.FromResult(true); // Default implementation
        }
    }
}
