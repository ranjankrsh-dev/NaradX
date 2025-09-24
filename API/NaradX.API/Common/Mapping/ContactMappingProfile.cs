using AutoMapper;
using NaradX.Business.Common.Mappings;
using NaradX.Business.Contacts.Commands.CreateContact;
using NaradX.Business.Contacts.Commands.UpdateContact;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Shared.Dto.Contact;
using NaradX.Shared.Models.Common;

namespace NaradX.API.Common.Mapping
{
    public class ContactMappingProfile : Profile
    {
        public ContactMappingProfile()
        {
            // Entity to DTO
            CreateMap<Contact, ContactDto>();

            // Command to Entity
            CreateMap<CreateContactCommand, Contact>();
            CreateMap<UpdateContactCommand, Contact>();

            // Specific PaginatedList mapping for Contacts
            CreateMap<PaginatedList<Contact>, PaginatedList<ContactDto>>()
                .ConvertUsing(new PaginatedListConverter<Contact, ContactDto>());

            CreateMap<Contact, ContactDto>()
    .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name))
    .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.Name))
    .ForMember(dest => dest.ImportSource, opt => opt.MapFrom(src => src.ImportSource.ToString()))
    .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ContactTags.Select(ct => ct.ToString()).ToList()));

            // Specific PaginatedList mapping for Contacts
            //CreateMap<PaginatedResult<Contact>, PaginatedResult<ContactDto>>()
            //    .ConvertUsing(new PaginatedListConverter<Contact, ContactDto>());
        }
    }

}
