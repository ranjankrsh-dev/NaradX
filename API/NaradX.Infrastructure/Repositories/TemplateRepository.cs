using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Infrastructure.Gateways.WhatsApp;
using NaradX.Infrastructure.Mappers;
using NaradX.Shared.Dto.Template;
using NaradX.Shared.Models;
using Refit;
using System.Text.Json;

namespace NaradX.Infrastructure.Repositories;

public class TemplateRepository(
    IWhatsAppApiGateway whatsAppApi,
    IOptions<WhatsAppOptions> options,
    ILogger<TemplateRepository> logger,
    NaradXDbContext context) : ITemplateRepository
{
    private readonly WhatsAppOptions _options = options.Value;

    // Cache JsonSerializerOptions to avoid CA1869
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

    // Implementation of the TemplateRepository class
    public async Task<CreateTemplateResponse> CreateWhatsAppMessageTemplate(WhatsAppMessageTemplateDTO whatsappMessageTemplate, CancellationToken cancellationToken)
    {
        try
        {
            // Log request details
            logger.LogInformation("Creating WhatsApp template with:\nBusinessId: {BusinessId}\nTemplate: {Template}",
                _options.BusinessId,
                JsonSerializer.Serialize(whatsappMessageTemplate, _jsonSerializerOptions));

            var response = await whatsAppApi.CreateTemplateAsync(
                _options.BusinessId,
                whatsappMessageTemplate,
                $"Bearer {_options.AccessToken}");

            logger.LogInformation("WhatsApp API Response: {Response}",
                JsonSerializer.Serialize(response, _jsonSerializerOptions));

            if (response != null)
            {
                var entity = WhatsAppTemplateMapper.ToEntity(whatsappMessageTemplate);
                await context.AddAsync(entity, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                logger.LogInformation("WhatsApp template saved to database with ID: {TemplateId}", entity.Id);
                return response;
            }

            // Return a default instance if response is null to avoid CS8603
            return new CreateTemplateResponse();
        }
        catch (ApiException ex)
        {
            logger.LogError("WhatsApp API error:\nStatusCode: {StatusCode}\nReason: {Reason}\nContent: {Content}\nHeaders: {Headers}",
                ex.StatusCode,
                ex.ReasonPhrase,
                ex.Content,
                JsonSerializer.Serialize(ex.Headers, _jsonSerializerOptions));
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating WhatsApp template");
            throw;
        }
    }
}
