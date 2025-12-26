// <copyright file="AssignRoleCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Auth.Roles.AssignRole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;

    public class AssignRoleCommand : IRequest<AssignRoleResponse>
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string AssignedBy { get; set; } = string.Empty;
    }
}
