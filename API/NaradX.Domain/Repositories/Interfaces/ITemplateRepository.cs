using NaradX.Shared.Dto.Template;
using NaradX.Shared.Models;

namespace NaradX.Domain.Repositories.Interfaces;

public interface ITemplateRepository
{
    Task<CreateTemplateResponse> CreateWhatsAppMessageTemplate(WhatsAppMessageTemplateDTO whatsappMessageTemplate, CancellationToken cancellationToken);
}
