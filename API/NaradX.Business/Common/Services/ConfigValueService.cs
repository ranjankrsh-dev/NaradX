using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Entities.Common;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Services
{
    public class ConfigValueService : IConfigValueService
    {
        private readonly IUnitOfWork _context;
        private readonly IMemoryCache _memoryCache;

        // Cache configuration
        private readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1)) // Cache for 1 hour after last access
            .SetAbsoluteExpiration(TimeSpan.FromHours(24)); // Absolute expiration after 24 hours

        public ConfigValueService(IUnitOfWork context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<IReadOnlyList<ConfigValueDto>> GetDropdownValuesAsync(string configKey, int? tenantId)
        {
            // 1. Create a unique cache key based on the config and tenant
            var cacheKey = $"Dropdown_{configKey}_{tenantId}";

            // 2. Try to get the result from the cache first
            if (_memoryCache.TryGetValue(cacheKey, out IReadOnlyList<ConfigValueDto> cachedResult))
            {
                return cachedResult;
            }

            // 3. If not cached, execute the database logic
            var result = await GetDropdownFromDatabaseAsync(configKey, tenantId);

            // 4. Store the result in the cache
            _memoryCache.Set(cacheKey, result, _cacheOptions);

            return result;
        }

        public async Task<Dictionary<string, IReadOnlyList<ConfigValueDto>>> GetMultipleDropdownValuesAsync(List<string> configKeys, int? tenantId)
        {
            if (configKeys == null || !configKeys.Any())
                return new Dictionary<string, IReadOnlyList<ConfigValueDto>>();

            // 1. Create a unique cache key for this bulk request
            var cacheKey = $"BulkDropdown_{string.Join("_", configKeys.OrderBy(k => k))}_{tenantId}";

            // 2. Try to get the result from the cache first
            if (_memoryCache.TryGetValue(cacheKey, out Dictionary<string, IReadOnlyList<ConfigValueDto>> cachedResult))
            {
                return cachedResult;
            }

            // 3. If not cached, execute the bulk database logic
            var result = await GetMultipleDropdownsFromDatabaseAsync(configKeys, tenantId);

            // 4. Store the result in the cache
            _memoryCache.Set(cacheKey, result, _cacheOptions);

            return result;
        }

        private async Task<IReadOnlyList<ConfigValueDto>> GetDropdownFromDatabaseAsync(string configKey, int? tenantId)
        {
            // 1. Find the Config Master entry
            var configMaster = await _context.GetRepository<ConfigMaster>()
                .FirstOrDefaultAsync(cm => cm.ConfigKey == configKey && cm.IsActive);

            if (configMaster == null)
            {
                // Handle the case where the config key doesn't exist
                // You could throw a specific exception like ConfigKeyNotFoundException
                return new List<ConfigValueDto>().AsReadOnly();
            }

            var valuesQuery = await _context.GetRepository<ConfigValue>()
                .FindAsync(cv => cv.ConfigMasterId == configMaster.Id && cv.IsActive);

            // 2. Apply the retrieval logic: Tenant-specific first, fallback to global
            if (configMaster.IsTenantSpecific && tenantId.HasValue)
            {
                // Try to get tenant-specific values
                var tenantValues = valuesQuery
                    .Where(cv => cv.TenantId == tenantId)
                    .OrderBy(cv => cv.DisplayOrder)
                    .ToList();

                if (tenantValues.Any())
                {
                    return tenantValues.Select(v => new ConfigValueDto(v.ItemValue, v.ItemText)).ToList().AsReadOnly();
                }
            }

            // 3. Fallback: Get global values (where TenantId is null)
            var globalValues = valuesQuery
                .Where(cv => cv.TenantId == null)
                .OrderBy(cv => cv.DisplayOrder)
                .ToList();

            return globalValues.Select(v => new ConfigValueDto(v.ItemValue, v.ItemText)).ToList().AsReadOnly();
        }

        private async Task<Dictionary<string, IReadOnlyList<ConfigValueDto>>> GetMultipleDropdownsFromDatabaseAsync(List<string> configKeys, int? tenantId)
        {
            var result = new Dictionary<string, IReadOnlyList<ConfigValueDto>>();

            // 1. Get all relevant Config Masters in one query
            var configMasters = await _context.GetRepository<ConfigMaster>()
                .FindAsync(cm => configKeys.Contains(cm.ConfigKey) && cm.IsActive);

            if (!configMasters.Any())
                return result;

            var masterIds = configMasters.Select(cm => cm.Id).ToList();

            // 2. Get ALL possible values for these configs in one query
            // We'll get both global (TenantId null) and tenant-specific values

            var allValues = await _context.GetRepository<ConfigValue>()
                .FindAsync(cv => masterIds.Contains(cv.ConfigMasterId) && cv.IsActive);

            // 3. For each requested config key, apply the logic: tenant-specific first, else global
            foreach (var configKey in configKeys)
            {
                var master = configMasters.FirstOrDefault(cm => cm.ConfigKey == configKey);
                if (master == null)
                {
                    // Config key not found, add an empty list
                    result[configKey] = new List<ConfigValueDto>().AsReadOnly();
                    continue;
                }

                var valuesForThisConfig = allValues.Where(v => v.ConfigMasterId == master.Id).ToList();

                IReadOnlyList<ConfigValueDto> finalValues;

                if (master.IsTenantSpecific && tenantId.HasValue)
                {
                    // Try to find tenant-specific values
                    var tenantValues = valuesForThisConfig
                        .Where(v => v.TenantId == tenantId)
                        .OrderBy(v => v.DisplayOrder)
                        .ToList();

                    if (tenantValues.Any())
                    {
                        finalValues = tenantValues.Select(v => new ConfigValueDto(v.ItemValue, v.ItemText)).ToList().AsReadOnly();
                    }
                    else
                    {
                        // Fallback to global values
                        finalValues = valuesForThisConfig
                            .Where(v => v.TenantId == null)
                            .OrderBy(v => v.DisplayOrder)
                            .Select(v => new ConfigValueDto(v.ItemValue, v.ItemText))
                            .ToList().AsReadOnly();
                    }
                }
                else
                {
                    // For non-tenant-specific configs, always use global values
                    finalValues = valuesForThisConfig
                        .Where(v => v.TenantId == null)
                        .OrderBy(v => v.DisplayOrder)
                        .Select(v => new ConfigValueDto(v.ItemValue, v.ItemText))
                        .ToList().AsReadOnly();
                }

                result[configKey] = finalValues;
            }

            return result;
        }
    }
}
