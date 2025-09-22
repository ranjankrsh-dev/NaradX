using NaradX.Domain.Entities.Template;

namespace NaradX.Infrastructure.Repositories;

public class TemplateRepository
{
    // Implementation of the TemplateRepository class
    public WhatsAppTemplate CreateOrderConfirmationTemplate()
    {
        return new WhatsAppTemplate
        {
            Name = "order_confirmation",
            Category = TemplateCategories.Transactional,
            Language = "en_US",
            Components = new List<Component>
            {
                new Component
                {
                    Type = ComponentTypes.Body,
                    Text = "Hi {{1}}, your order #{{2}} has been confirmed. Total: {{3}}"
                }
            }
        };
    }
}
