// <copyright file="ContactTag.cs" company="PlaceholderCompany">
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

    public class ContactTag:BaseEntity
    {
        // Composite primary key
        public int ContactId { get; set; }

        public int TagId { get; set; }

        // Navigation properties
        public virtual Contact Contact { get; set; } = null!;

        public virtual Tag Tag { get; set; } = null!;
    }
}
