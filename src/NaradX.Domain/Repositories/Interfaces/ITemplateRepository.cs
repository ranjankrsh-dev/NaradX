using NaradX.Domain.Entities.Template;


namespace NaradX.Domain.Repositories.Interfaces;

public interface ITemplateRepository
{
    // Ideally this should just take the Entity, but since it calls an API, we keep the DTO input for now 
    // or better, we change the input to be the Entity too.
    // However, the implementation uses DTO for serialization to API. 
    // Let's change output first.
    Task<NaradX.Domain.Entities.Template.WhatsAppTemplate> CreateWhatsAppMessageTemplateAsync(NaradX.Domain.Entities.Template.WhatsAppTemplate whatsappMessageTemplate, CancellationToken cancellationToken);
    Task<List<NaradX.Domain.Entities.Template.WhatsAppTemplate>> GetAllWhatsAppMessageTemplatesAsync(CancellationToken cancellationToken);
    Task<NaradX.Domain.Entities.Template.WhatsAppTemplate?> GetWhatsAppMessageTemplateByNameAsync(string templateName, CancellationToken cancellationToken);
    Task<bool> DeleteWhatsAppMessageTemplateByNameAsync(string templateName, CancellationToken cancellationToken);
}
