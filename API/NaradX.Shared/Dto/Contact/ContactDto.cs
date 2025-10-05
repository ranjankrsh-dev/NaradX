using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Shared.Dto.Contact
{
    public class ContactDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string FirstName { get; set; }= null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int CountryId { get; set; }
        public string Language { get; set; } = null!;
        public int LanguageId { get; set; }
        public string ContactSource { get; set; } = null!;
        public string ChannelPreference { get; set; } = null!;
        public string ImportSource { get; set; } = null!;
        public string? Email { get; set; }
        public string? Company { get; set; }
        public string? JobTitle { get; set; }
        public List<string> Tags { get; set; } = new();
        public string? Timezone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
