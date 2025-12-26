// <copyright file="AssignRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.Role;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AssignRoleRequest
{
    public int UserId { get; set; }

    public int RoleId { get; set; }
}
