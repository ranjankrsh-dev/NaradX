// <copyright file="WhatsAppBusinessConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Whatsapp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Base;
    using NaradX.Domain.Entities.Tenancy;

    public class WhatsAppBusinessConfig : BaseEntity
    {
        public int TenantId { get; set; }

        public virtual Tenant Tenant { get; set; } = null!; 
    }
}
