using NaradX.Shared.Dto.Template;
using NaradX.Shared.Models;

namespace NaradX.Domain.Repositories.Interfaces;

public interface ITemplateRepository
{
    Task<CreateTemplateResponse> CreateWhatsAppMessageTemplateAsync(WhatsAppMessageTemplateDTO whatsappMessageTemplate, CancellationToken cancellationToken);
    Task<List<WhatsAppMessageTemplateDTO>> GetAllWhatsAppMessageTemplatesAsync(CancellationToken cancellationToken);
    Task<WhatsAppMessageTemplateDTO?> GetWhatsAppMessageTemplateByNameAsync(string templateName, CancellationToken cancellationToken);
    Task<bool> DeleteWhatsAppMessageTemplateByNameAsync(string templateName, CancellationToken cancellationToken);
}
