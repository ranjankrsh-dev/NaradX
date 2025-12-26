// <copyright file="GetBulkUploadValidationQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Contacts.Queries.GetBulkUploadValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using NaradX.Business.Dtos.BulkUpload;

    public class GetBulkUploadValidationQuery : IRequest<BulkUploadValidateResponse>
    {
        public int TenantId { get; set; }

        public int CountryId { get; set; }

        public int LanguageId { get; set; }

        public string ContactSource { get; set; } = null!;

        public string ChannelPreference { get; set; } = null!;

        public IFormFile UploadedFile { get; set; } = null!;
    }
}
