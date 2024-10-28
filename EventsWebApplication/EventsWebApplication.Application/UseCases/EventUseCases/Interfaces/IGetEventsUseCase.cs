using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.GeneralUseCases.Interface;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces
{
    public interface IGetEventsUseCase : IGetManyUseCase<EventDto>
    {
    }
}
