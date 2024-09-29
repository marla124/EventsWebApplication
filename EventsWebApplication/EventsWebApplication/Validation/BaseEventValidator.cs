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

            RuleFor(eventInfo => eventInfo.CategoryName)
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

    public class UpdateEventValidator : BaseEventValidator<UpdateEventModel>
    {
        public UpdateEventValidator()
        {
            RuleFor(eventInfo => eventInfo.Id)
                .NotEmpty();
        }
    }
}
