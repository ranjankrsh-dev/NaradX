// <copyright file="ConfigValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Base;
    using NaradX.Domain.Entities.Tenancy;

    public class ConfigValue : ActivatableEntity
    {
        public int ConfigMasterId { get; set; }

        public virtual ConfigMaster ConfigMaster { get; set; } = null!;

        public int? TenantId { get; set; }

        public virtual Tenant? Tenant { get; set; }

        [Required]
        [MaxLength(255)]
        public string ItemText { get; set; } = null!; // e.g., "API", "WEBSITE" (the stored value)

        [Required]
        [MaxLength(255)]
        public string ItemValue { get; set; } = null!; // e.g., "API", "Website User" (the displayed label)

        public int DisplayOrder { get; set; } = 0;
    }
}
