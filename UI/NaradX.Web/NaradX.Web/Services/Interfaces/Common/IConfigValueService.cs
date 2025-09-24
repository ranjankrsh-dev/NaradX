using NaradX.Entities.Response.Common;

namespace NaradX.Web.Services.Interfaces.Common
{
    public interface IConfigValueService
    {
        Task<IReadOnlyList<ConfigValueDto>> GetConfigValuesAsync(string configKey, int? tenantId);
        Task<Dictionary<string, IReadOnlyList<ConfigValueDto>>> GetMultipleConfigValuesAsync(IEnumerable<string> configKeys, int tenantId);
    }
}
