// <copyright file="ContactProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using NaradX.Business.Contacts.Commands.CreateContact;
    using NaradX.Business.Contacts.Commands.UpdateContact;
    using NaradX.Domain.Entities.ManageContact;
    using NaradX.Business.Dtos.Contact;

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
