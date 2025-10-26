using MediatR;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Dto.Template;

namespace NaradX.Business.Template.Query;

public class GetAllTemplateQueryHandler(ITemplateRepository templateRepository) : IRequestHandler<GetAllTemplateQuery, List<WhatsAppMessageTemplateDTO>>
{
    public async Task<List<WhatsAppMessageTemplateDTO>> Handle(GetAllTemplateQuery request, CancellationToken cancellationToken)
    {
        return await templateRepository.GetAllWhatsAppMessageTemplatesAsync(cancellationToken);
    }
}
