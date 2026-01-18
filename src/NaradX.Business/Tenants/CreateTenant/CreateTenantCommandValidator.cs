using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Tenants.CreateTenant
{
    public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tenant name is required")
                .MaximumLength(200).WithMessage("Tenant name cannot exceed 200 characters");

            RuleFor(x => x.Subdomain)
                .NotEmpty().WithMessage("Subdomain is required")
                .MaximumLength(100).WithMessage("Subdomain cannot exceed 100 characters")
                .Matches("^[a-z0-9]+(-[a-z0-9]+)*$").WithMessage("Subdomain can only contain lowercase letters, numbers, and hyphens");
        }
    }
}
