// <copyright file="CreateTenantResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Tenants.CreateTenant
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateTenantResponse
    {
        public int TenantId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
