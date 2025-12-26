// <copyright file="IJwtService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Auth;

    public interface IJwtService
    {
        string GenerateToken(User user, IEnumerable<UserRole> role);

        int? ValidateToken(string token);
    }
}
