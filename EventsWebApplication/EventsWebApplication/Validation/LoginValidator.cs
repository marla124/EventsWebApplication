using EventsWebApplication.Models;
using FluentValidation;

namespace EventsWebApplication.Validation
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty().EmailAddress();

            RuleFor(user => user.Password)
                .NotEmpty().MinimumLength(8).MaximumLength(64);
        }
    }
}
