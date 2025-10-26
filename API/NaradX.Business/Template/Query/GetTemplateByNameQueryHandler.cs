using MediatR;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Dto.Template;

namespace NaradX.Business.Template.Query;

public class GetTemplateByNameQueryHandler(ITemplateRepository templateRepository) : IRequestHandler<GetTemplateByNameQuery, WhatsAppMessageTemplateDTO>
{
    public async Task<WhatsAppMessageTemplateDTO> Handle(GetTemplateByNameQuery request, CancellationToken cancellationToken)
    {
        var template = await templateRepository.GetWhatsAppMessageTemplateByNameAsync(request.Name, cancellationToken);
        return template ?? throw new InvalidOperationException($"Template with name '{request.Name}' was not found.");
    }
}
