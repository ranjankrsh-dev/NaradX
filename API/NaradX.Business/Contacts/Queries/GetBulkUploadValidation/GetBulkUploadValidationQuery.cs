using MediatR;
using Microsoft.AspNetCore.Http;
using NaradX.Shared.Dto.BulkUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Queries.GetBulkUploadValidation
{
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
