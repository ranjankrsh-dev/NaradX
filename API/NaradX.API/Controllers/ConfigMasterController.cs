using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Entities.Tenancy;
using NaradX.Shared.Dto.Common;

namespace NaradX.API.Controllers
{
    [Route("api/config-master")]
    [ApiController]
    public class ConfigMasterController : ControllerBase
    {
        private readonly ILogger<ConfigMasterController> _logger;
        private readonly IConfigValueService _configValueService;

        public ConfigMasterController(ILogger<ConfigMasterController> logger, IConfigValueService configValueService)
        {
            _logger = logger;
            _configValueService = configValueService;
        }

        [HttpGet("configKey/{configKey}")]
        public async Task<ActionResult<IReadOnlyList<ConfigValueDto>>> GetDropdownValues(string configKey, int? tenantId)
        {
            var values = await _configValueService.GetDropdownValuesAsync(configKey, tenantId);
            return Ok(values);
        }

        [HttpPost("get-all-config-values")]
        public async Task<ActionResult<Dictionary<string, IReadOnlyList<ConfigValueDto>>>> GetMultipleDropdownValues([FromBody] List<string> configKeys, int? tenantId)
        {
            var values = await _configValueService.GetMultipleDropdownValuesAsync(configKeys, tenantId);
            return Ok(values);
        }
    }
}
