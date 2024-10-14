using EventsWebApplication.Models;
using FluentValidation;

namespace EventsWebApplication.Validation
{
    public class BaseEventValidator<TModel> : AbstractValidator<TModel> where TModel : EventModel
    {
        public BaseEventValidator()
        {
            RuleFor(eventInfo => eventInfo.Name)
                .NotEmpty().MaximumLength(50);

            RuleFor(eventInfo => eventInfo.Name)
                .MaximumLength(250);

            RuleFor(eventInfo => eventInfo.CategoryId)
                .NotEmpty();

            RuleFor(eventInfo => eventInfo.Address)
                .MaximumLength(50);

            RuleFor(eventInfo => eventInfo.MaxNumberOfPeople)
                .NotEmpty();
        }
    }

    public class EventValidator : BaseEventValidator<EventModel>
    {
        public EventValidator()
        {
        }
    }
}
