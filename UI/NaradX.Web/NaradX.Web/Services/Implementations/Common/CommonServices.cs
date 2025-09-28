using NaradX.Entities.Response.Common;
using NaradX.Web.Services.Interfaces.Common;

namespace NaradX.Web.Services.Implementations.Common
{
    public class CommonServices : ICommonServices
    {
        private const string BaseUrl = "api/base";
        private readonly IApiHelper _apiHelper;
        private readonly ILogger<CommonServices> _logger;
        public CommonServices(IApiHelper apiHelper, ILogger<CommonServices> logger)
        {
            _apiHelper = apiHelper;
            _logger = logger;
        }
        public Task<IReadOnlyList<CountryDto>> GetAllCountriesAsync()
        {
            string endpoint = $"{BaseUrl}/countries-list";
            return _apiHelper.GetData<IReadOnlyList<CountryDto>>(endpoint);
        }

        public async Task<CountryDto> GetLanguagesByCountryIdAsync(int countryId)
        {
            string endpoint = $"{BaseUrl}/language-by-country/{countryId}";
            return await _apiHelper.GetData<CountryDto>(endpoint);
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
