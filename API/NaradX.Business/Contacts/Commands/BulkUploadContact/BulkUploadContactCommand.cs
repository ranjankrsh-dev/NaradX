// <copyright file="BulkUploadContactCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Contacts.Commands.BulkUploadContact
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;
    using NaradX.Business.Dtos.Common;

    public class BulkUploadContactCommand : IRequest<ResponseDto>
    {
        public string BatchId { get; set; } = null!;
    }
}
