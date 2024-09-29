using EventsWebApplication.Models;
using FluentValidation;

namespace EventsWebApplication.Validation
{
    public class RegisterValidator: AbstractValidator<RegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty().EmailAddress();

            RuleFor(user => user.Name)
                .NotEmpty().MaximumLength(30);

            RuleFor(user => user.Surname)
                .NotEmpty().MaximumLength(30);

            RuleFor(user => user.Password)
                .NotEmpty().MinimumLength(8).MaximumLength(64);

            RuleFor(user => user.PasswordConfirmation)
                .Equal(user => user.Password);

        }
    }
}
