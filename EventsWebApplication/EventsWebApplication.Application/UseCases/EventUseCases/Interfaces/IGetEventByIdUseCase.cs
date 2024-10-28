using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.GeneralUseCases.Interface;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces
{
    public interface IGetEventByIdUseCase : IGetByIdUseCase<EventDto>
    {

    }
}
