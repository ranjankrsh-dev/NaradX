using NaradX.Domain.Entities.Whatsapp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Interfaces
{
    public interface IWhatsAppSenderService
    {
        Task<(bool success, string response)> SendTemplateMessageAsync(WhatsAppBusinessConfig config, string to, string templateName, string languageCode = "en_US");
    }
}
