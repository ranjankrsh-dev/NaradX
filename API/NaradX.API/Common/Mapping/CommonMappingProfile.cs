using AutoMapper;
using NaradX.Domain.Entities.Common;
using NaradX.Shared.Dto.Common;

namespace NaradX.API.Common.Mapping
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile() 
        {
            CreateMap<Country, CountryDto>();
            CreateMap<Language, LanguageDto>();
            CreateMap<ConfigValue, ConfigValueDto>();

            CreateMap<Country, CountryDto>().ForMember(dest => dest.Languages,
               opt => opt.MapFrom(src => src.Languages));
        }
    }
}
