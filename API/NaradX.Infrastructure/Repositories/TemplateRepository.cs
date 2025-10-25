using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Infrastructure.Gateways.WhatsApp;
using NaradX.Shared.Dto.Template;
using NaradX.Shared.Models;
using Refit;
using System.Text.Json;

namespace NaradX.Infrastructure.Repositories;

public class TemplateRepository(
    IWhatsAppApiGateway whatsAppApi,
    IOptions<WhatsAppOptions> options,
    ILogger<TemplateRepository> logger) : ITemplateRepository
{
    private readonly IWhatsAppApiGateway _whatsAppApi = whatsAppApi;
    private readonly WhatsAppOptions _options = options.Value;

    // Implementation of the TemplateRepository class
    public async Task<CreateTemplateResponse> CreateWhatsAppMessageTemplate(WhatsAppMessageTemplateDTO whatsappMessageTemplate, CancellationToken cancellationToken)
    {
        try
        {
            // Log request details
            logger.LogInformation("Creating WhatsApp template with:\nBusinessId: {BusinessId}\nTemplate: {Template}",
                _options.BusinessId,
                JsonSerializer.Serialize(whatsappMessageTemplate, new JsonSerializerOptions { WriteIndented = true }));

            var response = await _whatsAppApi.CreateTemplateAsync(
                _options.BusinessId,
                whatsappMessageTemplate,
                $"Bearer {_options.AccessToken}");

            logger.LogInformation("WhatsApp API Response: {Response}",
                JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));

            return response;
        }
        catch (ApiException ex)
        {
            logger.LogError("WhatsApp API error:\nStatusCode: {StatusCode}\nReason: {Reason}\nContent: {Content}\nHeaders: {Headers}",
                ex.StatusCode,
                ex.ReasonPhrase,
                ex.Content,
                JsonSerializer.Serialize(ex.Headers, new JsonSerializerOptions { WriteIndented = true }));
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating WhatsApp template");
            throw;
        }
    }
}
