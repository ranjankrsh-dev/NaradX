// <copyright file="GetAllTemplateQueryHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Template.Query;

using MediatR;
using NaradX.Business.Dtos.Template;
using NaradX.Domain.Interfaces;

public class GetAllTemplateQueryHandler(ITemplateRepository templateRepository) : IRequestHandler<GetAllTemplateQuery, List<WhatsAppMessageTemplateDTO>>
{
    public async Task<List<WhatsAppMessageTemplateDTO>> Handle(GetAllTemplateQuery request, CancellationToken cancellationToken)
    {
        var result = await templateRepository.GetAllWhatsAppMessageTemplatesAsync(cancellationToken);
        return result.Cast<WhatsAppMessageTemplateDTO>().ToList();
    }
}
