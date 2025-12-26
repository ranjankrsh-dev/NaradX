// <copyright file="DeleteContactCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Contacts.Commands.DeleteContact
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;

    public class DeleteContactCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
