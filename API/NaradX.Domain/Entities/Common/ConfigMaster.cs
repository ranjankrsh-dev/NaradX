// <copyright file="ConfigMaster.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Base;

    public class ConfigMaster : ActivatableEntity
    {
        [Required]
        [MaxLength(255)]
        public string ConfigKey { get; set; } = null!; // e.g., "DATA_SOURCE", "CONTACT_SOURCE"

        [MaxLength(1000)]
        public string? Description { get; set; } = null!;

        public bool IsTenantSpecific { get; set; } = false;

        public virtual ICollection<ConfigValue> ConfigValues { get; set; } = new List<ConfigValue>();
    }
}
