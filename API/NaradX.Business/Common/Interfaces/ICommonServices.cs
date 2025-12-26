// <copyright file="ICommonServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Business.Dtos.Common;

    public interface ICommonServices
    {
        Task<IReadOnlyList<ConfigValueDto>> GetDropdownValuesAsync(string configKey, int? tenantId);

        Task<Dictionary<string, IReadOnlyList<ConfigValueDto>>> GetMultipleDropdownValuesAsync(List<string> configKeys, int? tenantId);

        void ClearDropdownCache(string? configKey = null, int? tenantId = null);

        Task<IReadOnlyList<CountryDto>> GetAllCountriesAsync();

        Task<CountryDto> GetLanguagesByCountryIdAsync(int countryId);
    }
}
