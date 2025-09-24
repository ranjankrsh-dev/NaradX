using NaradX.Entities.Response.Common;
using NaradX.Web.Services.Interfaces.Common;

namespace NaradX.Web.Services.Implementations.Common
{
    public class ConfigValueService : IConfigValueService
    {
        private const string BaseUrl = "api/config-master";
        private readonly IApiHelper _apiHelper;
        private readonly ILogger<ConfigValueService> _logger;
        public ConfigValueService(IApiHelper apiHelper, ILogger<ConfigValueService> logger)
        {
            _apiHelper = apiHelper;
            _logger = logger;
        }

        public Task<IReadOnlyList<ConfigValueDto>> GetConfigValuesAsync(string configKey, int? tenantId)
        {
            string endpoint = $"{BaseUrl}/configKey/{configKey}";
            if (tenantId.HasValue)
            {
                endpoint += $"?tenantId={tenantId.Value}";
            }
            return _apiHelper.GetData<IReadOnlyList<ConfigValueDto>>(endpoint);

        }

        public Task<Dictionary<string, IReadOnlyList<ConfigValueDto>>> GetMultipleConfigValuesAsync(IEnumerable<string> configKeys, int tenantId)
        {
            string endpoint = $"{BaseUrl}/get-all-config-values?tenantId={tenantId}";
            return _apiHelper.PostData<IEnumerable<string>, Dictionary<string, IReadOnlyList<ConfigValueDto>>>(endpoint, configKeys);
        }
    }
}
