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
}
