// <copyright file="GetUserRolesQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Auth.Roles.GetUserRoles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;

    public class GetUserRolesQuery : IRequest<GetUserRolesResponse>
    {
        public int UserId { get; set; }
    }
}
