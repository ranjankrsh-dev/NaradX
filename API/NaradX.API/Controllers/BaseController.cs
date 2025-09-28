using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaradX.Business.Common.Interfaces;
using NaradX.Business.Common.Services;
using NaradX.Domain.Entities.Common;
using NaradX.Shared.Dto.Common;

namespace NaradX.API.Controllers
{
    [Route("api/base")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly ILogger<BaseController> logger;
        private readonly ICommonServices commonServices;
        public BaseController(ILogger<BaseController> logger, ICommonServices commonServices)
        {
            this.logger = logger;
            this.commonServices = commonServices;
        }

        #region Config Master & Values

        [HttpGet("configKey/{configKey}")]
        public async Task<ActionResult<IReadOnlyList<ConfigValueDto>>> GetDropdownValues(string configKey, int? tenantId)
        {
            var values = await commonServices.GetDropdownValuesAsync(configKey, tenantId);
            return Ok(values);
        }

        [HttpPost("get-all-config-values")]
        public async Task<ActionResult<Dictionary<string, IReadOnlyList<ConfigValueDto>>>> GetMultipleDropdownValues([FromBody] List<string> configKeys, int? tenantId)
        {
            var values = await commonServices.GetMultipleDropdownValuesAsync(configKeys, tenantId);
            return Ok(values);
        }

        #endregion

        #region Country and Languages

        [HttpGet("countries-list")]
        public async Task<ActionResult<IReadOnlyList<CountryDto>>> GetAllCountries()
        {
            var countries = await commonServices.GetAllCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("language-by-country/{countryId}")]
        public async Task<ActionResult<CountryDto>> GetLanguageByCountryId(int countryId)
        {
            var language = await commonServices.GetLanguagesByCountryIdAsync(countryId);
            return Ok(language);
        }

        #endregion
    }
}
