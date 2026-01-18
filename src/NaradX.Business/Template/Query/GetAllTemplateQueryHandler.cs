using MediatR;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Business.Dto.Template;

namespace NaradX.Business.Template.Query;

public class GetAllTemplateQueryHandler(ITemplateRepository templateRepository, AutoMapper.IMapper mapper) : IRequestHandler<GetAllTemplateQuery, List<WhatsAppMessageTemplateDTO>>
{
    public async Task<List<WhatsAppMessageTemplateDTO>> Handle(GetAllTemplateQuery request, CancellationToken cancellationToken)
    {
        var templates = await templateRepository.GetAllWhatsAppMessageTemplatesAsync(cancellationToken);
        return mapper.Map<List<WhatsAppMessageTemplateDTO>>(templates);
    }
}
