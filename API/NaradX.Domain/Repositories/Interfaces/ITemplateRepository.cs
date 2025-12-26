// <copyright file="ITemplateRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Repositories.Interfaces;

public interface ITemplateRepository
{
    Task<object> CreateWhatsAppMessageTemplateAsync(object whatsappMessageTemplate, CancellationToken cancellationToken);

    Task<List<object>> GetAllWhatsAppMessageTemplatesAsync(CancellationToken cancellationToken);

    Task<object?> GetWhatsAppMessageTemplateByNameAsync(string templateName, CancellationToken cancellationToken);

    Task<bool> DeleteWhatsAppMessageTemplateByNameAsync(string templateName, CancellationToken cancellationToken);
}
