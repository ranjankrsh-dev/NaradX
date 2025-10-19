using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaradX.Domain.Entities.Whatsapp;
using NaradX.Infrastructure;

namespace NaradX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        public NaradXDbContext _db;
        public SetupController(NaradXDbContext db)
        {
            _db = db;
        }

        [HttpPost("saveconfig")]
        public async Task<IActionResult> SaveConfig([FromBody] SaveConfigRequest input)
        {
            if (input == null) return BadRequest("Input required");

            WhatsAppBusinessConfig configObj = new WhatsAppBusinessConfig();
            configObj.TenantId = input.TenantId;
            configObj.AccessToken = input.AccessToken;
            configObj.PhoneNumberId = input.PhoneNumberId;
            configObj.WabaId = input.WabaId;


            var config = await _db.WhatsAppConfigs.FirstOrDefaultAsync(c => c.Id == input.TenantId);
            if (config == null)
            {
                _db.WhatsAppConfigs.Add(configObj);
            }
            else
            {
                config.TenantId = input.TenantId;
                config.AccessToken = input.AccessToken;
                config.PhoneNumberId = input.PhoneNumberId;
                config.WabaId = input.WabaId;
                config.UpdatedOn = DateTime.UtcNow;
                _db.WhatsAppConfigs.Update(config);
            }


            await _db.SaveChangesAsync();
            return Ok(new { success = true });
        }


        [HttpGet("getconfig")]
        public async Task<IActionResult> GetConfig()
        {
            var config = await _db.WhatsAppConfigs.FirstOrDefaultAsync(c => c.Id == 1);
            if (config == null) return NotFound();
            // DON'T return token in real production. For testing it's okay.
            return Ok(config);
        }
    }

    public class SaveConfigRequest
    {
        public int TenantId { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string PhoneNumberId { get; set; } = string.Empty; // Graph phone number id
        public string WabaId { get; set; } = string.Empty; // WhatsApp Business Account ID
    }
}
