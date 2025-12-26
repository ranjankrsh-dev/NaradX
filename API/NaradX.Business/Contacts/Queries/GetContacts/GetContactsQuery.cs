// <copyright file="GetContactsQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Contacts.Queries.GetContacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;
    using NaradX.Business.Dtos.Contact;
    using NaradX.Domain.Common;

    public class GetContactsQuery : IRequest<PaginatedList<ContactDto>>
    {
        public ContactFilterParams ContactFilter { get; set; }

        public GetContactsQuery(ContactFilterParams ContactFilter)
        {
            this.ContactFilter = ContactFilter;
        }
    }
}
