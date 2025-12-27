// <copyright file="GetTemplateByNameQueryHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Template.Query;

using MediatR;
using NaradX.Business.Dtos.Template;
using NaradX.Domain.Interfaces;

public class GetTemplateByNameQueryHandler(ITemplateRepository templateRepository) : IRequestHandler<GetTemplateByNameQuery, WhatsAppMessageTemplateDTO>
{
    public async Task<WhatsAppMessageTemplateDTO> Handle(GetTemplateByNameQuery request, CancellationToken cancellationToken)
    {
        var template = await templateRepository.GetWhatsAppMessageTemplateByNameAsync(request.Name, cancellationToken);
        return (WhatsAppMessageTemplateDTO)(template ?? throw new InvalidOperationException($"Template with name '{request.Name}' was not found."));
    }
}
