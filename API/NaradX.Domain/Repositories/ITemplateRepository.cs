using NaradX.Domain.Entities.Template;

namespace NaradX.Domain.Repositories;

public interface ITemplateRepository
{
    Task<WhatsAppTemplate> CreateOrderConfirmationTemplateAsync();
}
