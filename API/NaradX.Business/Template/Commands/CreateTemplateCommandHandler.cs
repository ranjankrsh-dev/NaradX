// <copyright file="CreateTemplateCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Template.Commands;

using MediatR;
using NaradX.Business.Dtos.Template;
using NaradX.Domain.Repositories.Interfaces;

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, CreateTemplateResponse>
{
    private readonly ITemplateRepository _templateRepository;

    public CreateTemplateCommandHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<CreateTemplateResponse> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _templateRepository.CreateWhatsAppMessageTemplateAsync(request.WhatsAppTemplate, cancellationToken);
        return (CreateTemplateResponse)result;
    }
}
