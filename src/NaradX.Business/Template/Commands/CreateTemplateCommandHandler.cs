using MediatR;
using NaradX.Business.Models;
using NaradX.Domain.Entities.Template;
using NaradX.Domain.Repositories.Interfaces;

namespace NaradX.Business.Template.Commands;

public class CreateTemplateCommandHandler(ITemplateRepository _templateRepository, AutoMapper.IMapper mapper) : IRequestHandler<CreateTemplateCommand, CreateTemplateResponse>
{
    public async Task<CreateTemplateResponse> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<WhatsAppTemplate>(request.WhatsAppTemplate);
        var result = await _templateRepository.CreateWhatsAppMessageTemplateAsync(entity, cancellationToken);
        return new CreateTemplateResponse();
    }
}
