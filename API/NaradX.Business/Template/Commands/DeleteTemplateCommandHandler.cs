// <copyright file="DeleteTemplateCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Template.Commands;

using MediatR;
using NaradX.Domain.Repositories.Interfaces;

public class DeleteTemplateCommandHandler(ITemplateRepository templateRepository) : IRequestHandler<DeleteTemplateCommand, bool>
{
    public async Task<bool> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
    {
        return await templateRepository.DeleteWhatsAppMessageTemplateByNameAsync(request.Name, cancellationToken);
    }
}
