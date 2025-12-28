using AutoMapper;
using NaradX.Domain.Entities.Template;
using NaradX.Business.Dto.Template;

namespace NaradX.Business.Mappings;

public class TemplateProfile : Profile
{
    public TemplateProfile()
    {
        CreateMap<WhatsAppTemplate, WhatsAppMessageTemplateDTO>()
            .ReverseMap();

        CreateMap<Component, ComponentDTO>()
            .ReverseMap();
    }
}
