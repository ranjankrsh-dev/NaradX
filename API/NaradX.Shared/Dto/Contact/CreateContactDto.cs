using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Shared.Dto.Contact
{
    public class CreateContactDto
    {
        public int TenantId { get; set; }
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string ContactSource { get; set; } = null!;
        public string? Email { get; set; }
        public int CountryId { get; set; }
        public int LanguageId { get; set; }
        public string? Company { get; set; }
        public string? JobTitle { get; set; }
        public List<string>? Tags { get; set; }
    }
}
