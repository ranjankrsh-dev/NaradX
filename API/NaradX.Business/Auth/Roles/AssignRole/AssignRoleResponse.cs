// <copyright file="AssignRoleResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Auth.Roles.AssignRole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AssignRoleResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
