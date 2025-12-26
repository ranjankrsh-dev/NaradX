// <copyright file="Tag.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.ManageContact
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Base;

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
