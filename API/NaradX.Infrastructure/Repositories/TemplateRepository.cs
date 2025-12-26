// <copyright file="TemplateRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Infrastructure.Repositories;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NaradX.Business.Dtos.Template;
using NaradX.Domain.Entities.Template;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Infrastructure.Gateways.WhatsApp;
using NaradX.Infrastructure.Mappers;
using Refit;

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
    public async Task<object> CreateWhatsAppMessageTemplateAsync(object whatsappMessageTemplate, CancellationToken cancellationToken)
    {
        try
        {
            var dto = whatsappMessageTemplate as WhatsAppMessageTemplateDTO ?? throw new ArgumentException("Invalid template format");

            // Log request details
            logger.LogInformation(
                "Creating WhatsApp template with:\nBusinessId: {BusinessId}\nTemplate: {Template}",
                _options.BusinessId,
                JsonSerializer.Serialize(dto, _jsonSerializerOptions));

            var response = await whatsAppApi.CreateTemplateAsync(
                _options.BusinessId,
                dto,
                $"Bearer {_options.AccessToken}");

            logger.LogInformation("WhatsApp API Response: {Response}",
                JsonSerializer.Serialize(response, _jsonSerializerOptions));

            if (response != null)
            {
                var entity = WhatsAppTemplateMapper.ToEntity(dto);
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

    public async Task<List<object>> GetAllWhatsAppMessageTemplatesAsync(CancellationToken cancellationToken)
    {
        var templates = context.WhatsAppTemplates
            .Select(WhatsAppTemplateMapper.ToDTO)
            .ToList();
        return await Task.FromResult(templates.Cast<object>().ToList());
    }

    public async Task<object?> GetWhatsAppMessageTemplateByNameAsync(string templateName, CancellationToken cancellationToken)
    {
        var template = await context.WhatsAppTemplates.FirstOrDefaultAsync(x => x.Name == templateName, cancellationToken: cancellationToken);
        return template != null ? (object)WhatsAppTemplateMapper.ToDTO(template) : null;
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

    public async Task<bool> SendWhatsAppTextMessage(string phoneNumberId, string recipientPhone)
    {
        // return await whatsAppApi.SendTextMessageAsync(
        //     phoneNumberId,
        //     new
        //     {
        //         to = recipientPhone,
        //         type = "text",
        //         text = new
        //         {
        //             body = "This is a test message from NaradX."
        //         }
        //     },
        //     $"Bearer {_options.AccessToken}").ConfigureAwait(false);
        return true;
    }
}
