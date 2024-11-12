using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces
{
    public interface IGetEventsUseCase
    {
        public Task<EventDto[]> Execute(CancellationToken cancellationToken);
    }
}
