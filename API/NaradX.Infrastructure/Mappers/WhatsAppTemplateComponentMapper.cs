using NaradX.Domain.Entities.Template;
using NaradX.Shared.Dto.Template;
using System.Collections.Generic;
using System.Linq;

namespace NaradX.Infrastructure.Mappers;

public class WhatsAppTemplateComponentMapper
{
    public static IList<Component> ToEntity(IList<ComponentDTO> componentDTO)
    {
        // Map each ComponentDTO to a new Component instance
        return [.. componentDTO
            .Select(dto => new Component
            {
                Type = dto.Type,
                Text = dto.Text
            })];
    }

    public static IList<ComponentDTO> ToDTO(IList<Component> components)
    {
        // Map each Component to a new ComponentDTO instance
        return [.. components
            .Select(entity => new ComponentDTO
            {
                Type = entity.Type,
                Text = entity.Text
            })];
    }
}
