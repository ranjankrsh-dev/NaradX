using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Entities.Whatsapp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Services
{
    public class WhatsAppSenderService : IWhatsAppSenderService
    {
        private readonly HttpClient _client;


        public WhatsAppSenderService(HttpClient client)
        {
            _client = client;
        }

        public async Task<(bool success, string response)> SendTemplateMessageAsync(WhatsAppBusinessConfig config, string to, string templateName, string languageCode = "en_US")
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (string.IsNullOrWhiteSpace(config.AccessToken)) return (false, "AccessToken not configured");
            if (string.IsNullOrWhiteSpace(config.PhoneNumberId)) return (false, "PhoneNumberId not configured");


            var url = $"https://graph.facebook.com/v22.0/{config.PhoneNumberId}/messages";


            var payload = new
            {
                messaging_product = "whatsapp",
                to = to,
                type = "template",
                template = new
                {
                    name = templateName,
                    language = new { code = languageCode }
                }
            };


            var json = JsonSerializer.Serialize(payload);
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");


            try
            {
                var response = await _client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return (true, responseContent);
                }
                else
                {
                    return (false, responseContent);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
