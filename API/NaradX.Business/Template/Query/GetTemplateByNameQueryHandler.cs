using MediatR;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Business.Dto.Template;

namespace NaradX.Business.Template.Query;

public class GetTemplateByNameQueryHandler(ITemplateRepository templateRepository, AutoMapper.IMapper mapper) : IRequestHandler<GetTemplateByNameQuery, WhatsAppMessageTemplateDTO>
{
    public async Task<WhatsAppMessageTemplateDTO> Handle(GetTemplateByNameQuery request, CancellationToken cancellationToken)
    {
        var template = await templateRepository.GetWhatsAppMessageTemplateByNameAsync(request.Name, cancellationToken);
        if (template == null) throw new InvalidOperationException($"Template with name '{request.Name}' was not found.");
        return mapper.Map<WhatsAppMessageTemplateDTO>(template);
    }
}
