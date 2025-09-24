using NaradX.Domain.Entities.Base;
using NaradX.Domain.Entities.Common;
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
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(100)]
        public string? Email { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; } = null!;

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; } = null!;

        [MaxLength(50)]
        public string? Timezone { get; set; }

        [MaxLength(100)]
        public string? Company { get; set; }

        [MaxLength(50)]
        public string? Title { get; set; }

        [MaxLength(30)]
        public string? InstagramHandle { get; set; }

        [MaxLength(100)]
        public string? FacebookPsid { get; set; }

        public ImportSource ImportSource { get; set; } = ImportSource.Manual;

        [MaxLength(100)]
        public string ContactSource { get; set; } = null!;

        [MaxLength(100)]
        public string ChannelPreference { get; set; } = null!;

        // ===== COMPLIANCE & PREFERENCES =====
        public OptInStatus OptInStatus { get; set; } = OptInStatus.OptedIn;
        public DateTime? OptInDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? OptInSource { get; set; }

        public virtual ICollection<ChannelPreference> ChannelPreferences { get; set; } = new List<ChannelPreference>();
        public virtual ICollection<ContactTag> ContactTags { get; set; } = new List<ContactTag>();
    }
}
