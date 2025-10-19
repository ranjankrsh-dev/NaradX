using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Entities.Whatsapp;
using NaradX.Infrastructure;
using System;

namespace NaradX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        private readonly NaradXDbContext _db;
        private readonly IWhatsAppSenderService _sender;


        public WhatsAppController(NaradXDbContext db, IWhatsAppSenderService sender)
        {
            _db = db;
            _sender = sender;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendRequest request)
        {
            if (request == null) return BadRequest("Input required");
            if (string.IsNullOrWhiteSpace(request.To)) return BadRequest("To number required");
            if (string.IsNullOrWhiteSpace(request.TemplateName)) return BadRequest("Template name required");


            var config = await _db.WhatsAppConfigs.FirstOrDefaultAsync(c => c.Id == 1);
            if (config == null) return BadRequest("WhatsApp not configured. Call /api/setup/saveconfig first.");


            var log = new MessageLog
            {
                ToNumber = request.To,
                TemplateName = request.TemplateName,
                LanguageCode = request.LanguageCode,
                Status = "Queued",
                CreatedOn = DateTime.UtcNow
            };


            _db.MessageLogs.Add(log);
            await _db.SaveChangesAsync();


            var (success, response) = await _sender.SendTemplateMessageAsync(config, request.To, request.TemplateName, request.LanguageCode);


            log.Status = success ? "Sent" : "Failed";
            log.Response = response;
            await _db.SaveChangesAsync();


            return Ok(new { success, response });
        }

    }

    public class SendRequest
    {
        public string To { get; set; } = string.Empty; // E.164 format
        public string TemplateName { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = "en_US";
    }
}
