using AutoMapper;
using NaradX.Business.Contacts.Commands.CreateContact;
using NaradX.Business.Contacts.Commands.UpdateContact;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Shared.Dto.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Mappings
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactDto>();

            CreateMap<CreateContactCommand, Contact>();
            CreateMap<UpdateContactCommand, Contact>();
        }
    }
}
