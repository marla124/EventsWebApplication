using EventsWebApplication.Models;
using FluentValidation;

namespace EventsWebApplication.Validation
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequestViewModel>
    {
        public UpdateUserValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty().EmailAddress();

            RuleFor(user => user.Name)
                .NotEmpty().MaximumLength(30);

            RuleFor(user => user.Surname)
                .NotEmpty().MaximumLength(30);

            RuleFor(user => user.Password)
                .NotEmpty().MinimumLength(8).MaximumLength(64);
        }
    }
}
