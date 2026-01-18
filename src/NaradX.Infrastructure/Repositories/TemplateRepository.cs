using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Infrastructure.Gateways.WhatsApp;
using NaradX.Infrastructure.Mappers;
using NaradX.Business.Dto.Template;
using NaradX.Business.Models;
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
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    // Implementation of the TemplateRepository class
    public async Task<NaradX.Domain.Entities.Template.WhatsAppTemplate> CreateWhatsAppMessageTemplateAsync(NaradX.Domain.Entities.Template.WhatsAppTemplate entity, CancellationToken cancellationToken)
    {
        try
        {
            var whatsappMessageTemplate = WhatsAppTemplateMapper.ToDTO(entity);

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
                // entity is already passed in.
                // Ideally we should capture the external ID from 'response.Id' if the entity supported it.
                // entity.ExternalId = response.Id; 
                
                await context.AddAsync(entity, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                logger.LogInformation("WhatsApp template saved to database with ID: {TemplateId}", entity.Id);
                return entity;
            }

            throw new ApplicationException("Failed to create template on WhatsApp.");
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

    public Task<List<NaradX.Domain.Entities.Template.WhatsAppTemplate>> GetAllWhatsAppMessageTemplatesAsync(CancellationToken cancellationToken)
    {
        var templates = context.WhatsAppTemplates
            .Include(x => x.Components) // Include usage related data if needed
            .ToList();
        return Task.FromResult(templates);
    }

    public async Task<NaradX.Domain.Entities.Template.WhatsAppTemplate?> GetWhatsAppMessageTemplateByNameAsync(string templateName, CancellationToken cancellationToken)
    {
        var template = await context.WhatsAppTemplates
            .Include(x => x.Components)
            .FirstOrDefaultAsync(x => x.Name == templateName, cancellationToken: cancellationToken);
        return template;
    }

    public async Task<bool> DeleteWhatsAppMessageTemplateByNameAsync(string templateName, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Deleting WhatsApp template with Name: {TemplateName}", templateName);
            var response = await whatsAppApi.DeleteTemplateAsync(
                _options.BusinessId,
                templateName,
                $"Bearer {_options.AccessToken}");
            logger.LogInformation("WhatsApp API Delete Response - Status: {StatusCode}, Content: {Content}",
                response?.StatusCode,
                response?.Content);
            var templateEntity = await context.WhatsAppTemplates.FirstOrDefaultAsync(x => x.Name == templateName, cancellationToken);
            if (response != null && templateEntity != null)
            {
                context.WhatsAppTemplates.Remove(templateEntity);
                await context.SaveChangesAsync(cancellationToken);
                logger.LogInformation("WhatsApp template with Name: {TemplateName} deleted from database", templateName);
            }
            return response != null && response.IsSuccessStatusCode;
        }
        catch (ApiException ex)
        {
            logger.LogError("WhatsApp API error during deletion:\nStatusCode: {StatusCode}\nReason: {Reason}\nContent: {Content}\nHeaders: {Headers}",
                ex.StatusCode,
                ex.ReasonPhrase,
                ex.Content,
                JsonSerializer.Serialize(ex.Headers, _jsonSerializerOptions));
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting WhatsApp template with Name: {TemplateName}", templateName);
            throw;
        }
    }
}
