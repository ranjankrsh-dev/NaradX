using MediatR;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Models;

namespace NaradX.Business.Template.Commands;

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, CreateTemplateResponse>
{
    private readonly ITemplateRepository _templateRepository;

    public CreateTemplateCommandHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<CreateTemplateResponse> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _templateRepository.CreateWhatsAppMessageTemplate(request.WhatsAppTemplate, cancellationToken);
    }
}
