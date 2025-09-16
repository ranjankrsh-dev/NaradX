using NaradX.Domain.Entities.Base;
using NaradX.Domain.Entities.Tenancy;
using NaradX.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.ManageContact
{
    public class Contact : FullAuditableEntity
    {
        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; } = null!;

        // Required Fields
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [MaxLength(500)]
        public string DisplayName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(5)]
        public string CountryCode { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<ChannelPreference> ChannelPreferences { get; set; } = new List<ChannelPreference>();
        public virtual ICollection<ContactTag> ContactTags { get; set; } = new List<ContactTag>();

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(30)]
        public string? InstagramHandle { get; set; }

        [MaxLength(100)]
        public string? FacebookPsid { get; set; }

        [MaxLength(100)]
        public string? Company { get; set; }

        [MaxLength(50)]
        public string? Title { get; set; }

        // Preferences
        [MaxLength(10)]
        public string LanguagePreference { get; set; } = "en";

        [MaxLength(50)]
        public string? Timezone { get; set; }

        public ContactSource Source { get; set; } = ContactSource.Manual;

        // Compliance
        public OptInStatus OptInStatus { get; set; } = OptInStatus.OptedIn;
        public DateTime? OptInDate { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string? OptInSource { get; set; }
    }
}
