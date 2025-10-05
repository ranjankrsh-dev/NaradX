using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Entities.Request.Contact
{
    public class BulkUploadValidateRequest
    {
        public int TenantId { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Country is required")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Language is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Language is required")]
        public int LanguageId { get; set; }

        [Required(ErrorMessage = "Contact source is required")]
        public required string ContactSource { get; set; }

        [Required(ErrorMessage = "Channel preference is required")]
        public required string ChannelPreference { get; set; }

        [Required(ErrorMessage = "Bulk upload template is required")]
        public required IFormFile UploadedFile { get; set; }
    }
}
