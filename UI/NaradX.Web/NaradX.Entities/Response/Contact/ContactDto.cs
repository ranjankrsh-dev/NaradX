using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Entities.Response.Contact
{
    public class ContactDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = null!;
        public string? DisplayName { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number is not valid.")]
        public string PhoneNumber { get; set; } = null!;
        public string? Country { get; set; }
        [Required(ErrorMessage = "Please select country")]
        public int CountryId { get; set; }
        public string? Language { get; set; }
        [Required(ErrorMessage = "Please select language")]
        public int LanguageId { get; set; }
        public string ContactSource { get; set; } = null!;
        public string ChannelPreference { get; set; } = null!;
        public string? ImportSource { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string? Email { get; set; }

        [MaxLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        public string? Company { get; set; }

        [MaxLength(50, ErrorMessage = "Company name cannot exceed 50 characters.")]
        public string? JobTitle { get; set; }
        public List<string> Tags { get; set; } = new();
        public string? Timezone { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
