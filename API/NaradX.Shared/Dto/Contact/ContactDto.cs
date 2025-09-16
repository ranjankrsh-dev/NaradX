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
        public string ContactName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string? Email { get; set; }
        public string? Company { get; set; }
        public string? Title { get; set; }
        public List<string> Tags { get; set; } = new();
        public string LanguagePreference { get; set; } = "en";
        public string? Timezone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
