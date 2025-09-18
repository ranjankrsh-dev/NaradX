using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Entities.Response.Contact
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string ContactSource { get; set; } = null!;
        public string ImportSource { get; set; } = null!;
        public string? Email { get; set; }
        public string? Company { get; set; }
        public string? Title { get; set; }
        public List<string> Tags { get; set; } = new();
        public string? Timezone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
