// <copyright file="GetContactByIdQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Contacts.Queries.GetContactById
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;
    using NaradX.Business.Dtos.Contact;

    public class GetContactByIdQuery : IRequest<ContactDto?>
    {
        public int Id { get; set; }
    }
}
