using NaradX.Shared.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Interfaces
{
    public interface IConfigValueService
    {
        Task<IReadOnlyList<ConfigValueDto>> GetDropdownValuesAsync(string configKey, int? tenantId);
        Task<Dictionary<string, IReadOnlyList<ConfigValueDto>>> GetMultipleDropdownValuesAsync(List<string> configKeys, int? tenantId);
    }
}
