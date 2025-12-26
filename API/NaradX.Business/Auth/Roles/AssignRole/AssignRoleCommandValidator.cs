// <copyright file="AssignRoleCommandValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Auth.Roles.AssignRole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentValidation;

    public class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
    {
        public AssignRoleCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Valid user ID is required");

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("Valid role ID is required");

            RuleFor(x => x.AssignedBy)
                .NotEmpty().WithMessage("Assigned by is required");
        }
    }
}
