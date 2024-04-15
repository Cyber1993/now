using FluentValidation;
using Core.Models.User;

namespace Core.Validators.User
{
    public class EditUserValidator : AbstractValidator<EditUserDto>
    {
        public EditUserValidator()
        {
            RuleFor(dto => dto.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MinimumLength(3).WithMessage("UserName must be at least 3 characters long.")
                .MaximumLength(255).WithMessage("UserName cannot exceed 50 characters.");

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid Email address.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

            RuleFor(dto => dto.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Matches(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$").WithMessage("Invalid PhoneNumber format.");
        }
    }
}
