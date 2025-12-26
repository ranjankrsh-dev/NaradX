// <copyright file="RemoveRoleCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Auth.Roles.RemoveRole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;

    public class RemoveRoleCommand : IRequest<RemoveRoleResponse>
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string RemovedBy { get; set; } = string.Empty;
    }
}
