using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Valid user ID is required");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("New password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("New password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("New password must contain at least one digit")
                .Matches("[!@#$%^&*(),.?\":{}|<>]").WithMessage("New password must contain at least one special character")
                .NotEqual(x => x.CurrentPassword).WithMessage("New password cannot be the same as current password");
        }
    }
}
