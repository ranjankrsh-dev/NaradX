using NaradX.Domain.Entities.Template;
using NaradX.Shared.Dto.Template;

namespace NaradX.Infrastructure.Mappers;

public class WhatsAppTemplateMapper
{
    public static WhatsAppTemplate ToEntity(WhatsAppMessageTemplateDTO whatsAppMessageTemplateDTO)
    {
        return new WhatsAppTemplate
        {
            Category = whatsAppMessageTemplateDTO.Category,
            Language = whatsAppMessageTemplateDTO.Language,
            Name = whatsAppMessageTemplateDTO.Name,
            Components = whatsAppMessageTemplateDTO.Components == null
                ? []
                : [.. WhatsAppTemplateComponentMapper.ToEntity(whatsAppMessageTemplateDTO.Components)]
        };
    }

    public static WhatsAppMessageTemplateDTO ToDTO(WhatsAppTemplate whatsAppTemplate)
    {
        return new WhatsAppMessageTemplateDTO
        {
            Category = whatsAppTemplate.Category,
            Language = whatsAppTemplate.Language,
            Name = whatsAppTemplate.Name,
            Components = whatsAppTemplate.Components == null
                ? []
                : [.. WhatsAppTemplateComponentMapper.ToDTO(whatsAppTemplate.Components)]
        };
    }
}
