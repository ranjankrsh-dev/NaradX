// <copyright file="ChannelPreference.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.ManageContact
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Base;
    using NaradX.Domain.Enums;

    public class ChannelPreference : FullAuditableEntity
    {
        public ChannelType ChannelType { get; set; }

        public DateTime? LastMessaged { get; set; }

        public string? ChannelSpecificId { get; set; } // PSID for Facebook, etc.

        // Foreign key to Contact
        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; } = null!;
    }
}
