using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Shared.Dto.BulkUpload
{
    public class ContactTemplateDto
    {
        public int RowNumber { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string? Company { get; set; }
        public string? JobTitle { get; set; }
    }
}
