// <copyright file="ITenantService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITenantService
    {
        int? TenantId { get; }

        string? TenantName { get; }

        Task<bool> SetTenantFromClaimsAsync(ClaimsPrincipal user);

        Task<bool> ValidateUserTenantAccessAsync(int userId, int tenantId);
    }
}
