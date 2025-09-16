using NaradX.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.ManageContact
{
    public class Tag : FullAuditableEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(200)]
        public string? Description { get; set; }

        [MaxLength(7)]
        public string Color { get; set; } = "#3B82F6"; // Default blue

        // Navigation property
        public virtual ICollection<ContactTag> ContactTags { get; set; } = new List<ContactTag>();
    }
}
