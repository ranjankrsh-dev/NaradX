using AutoMapper;
using NaradX.Business.Common.Mappings;
using NaradX.Business.Contacts.Commands.CreateContact;
using NaradX.Business.Contacts.Commands.UpdateContact;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Shared.Common.Model;
using NaradX.Shared.Dto.Contact;

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
        }
    }

}
