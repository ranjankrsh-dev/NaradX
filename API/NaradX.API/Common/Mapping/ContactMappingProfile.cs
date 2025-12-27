// <copyright file="ContactMappingProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.API.Common.Mapping;

using AutoMapper;
using NaradX.Business.Mappings;
using NaradX.Business.Contacts.Commands.CreateContact;
using NaradX.Business.Contacts.Commands.UpdateContact;
using NaradX.Business.Dtos.Contact;
using NaradX.Domain.Common;
using NaradX.Domain.Entities.ManageContact;

public class ContactMappingProfile : Profile
{
    public ContactMappingProfile()
    {
        // Entity to DTO
        this.CreateMap<Contact, ContactDto>();

        // Command to Entity
        this.CreateMap<CreateContactCommand, Contact>();
        this.CreateMap<UpdateContactCommand, Contact>();

        // Specific PaginatedList mapping for Contacts
        this.CreateMap<PaginatedList<Contact>, PaginatedList<ContactDto>>()
            .ConvertUsing(new PaginatedListConverter<Contact, ContactDto>());

        this.CreateMap<Contact, ContactDto>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.Name))
            .ForMember(dest => dest.ImportSource, opt => opt.MapFrom(src => src.ImportSource.ToString()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ContactTags.Select(ct => ct.ToString()).ToList()));
    }
}
