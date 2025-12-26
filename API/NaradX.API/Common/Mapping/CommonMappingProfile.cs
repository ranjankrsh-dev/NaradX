// <copyright file="CommonMappingProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.API.Common.Mapping;

using AutoMapper;
using NaradX.Business.Dtos.Common;
using NaradX.Domain.Entities.Common;

public class CommonMappingProfile : Profile
{
    public CommonMappingProfile()
    {
        this.CreateMap<Country, CountryDto>();
        this.CreateMap<Language, LanguageDto>();
        this.CreateMap<ConfigValue, ConfigValueDto>();

        this.CreateMap<Country, CountryDto>().ForMember(
            dest => dest.Languages,
            opt => opt.MapFrom(src => src.Languages));
    }
}