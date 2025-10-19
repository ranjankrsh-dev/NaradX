using NaradX.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Whatsapp
{
    public class MessageLog : BaseEntity
    {
        public string ToNumber { get; set; } = string.Empty; // E.164
        public string TemplateName { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = "en_US";
        public string Status { get; set; } = "Queued"; // Queued, Sent, Failed
        public string? Response { get; set; }
    }
}
