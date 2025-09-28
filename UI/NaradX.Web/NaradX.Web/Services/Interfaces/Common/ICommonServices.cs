using NaradX.Entities.Response.Common;

namespace NaradX.Web.Services.Interfaces.Common
{
    public interface ICommonServices
    {
        Task<IReadOnlyList<ConfigValueDto>> GetConfigValuesAsync(string configKey, int? tenantId);
        Task<Dictionary<string, IReadOnlyList<ConfigValueDto>>> GetMultipleConfigValuesAsync(IEnumerable<string> configKeys, int tenantId);
        Task<IReadOnlyList<CountryDto>> GetAllCountriesAsync();
        Task<CountryDto> GetLanguagesByCountryIdAsync(int countryId);
    }
}
