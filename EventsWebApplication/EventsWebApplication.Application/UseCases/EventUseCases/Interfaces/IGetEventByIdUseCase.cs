using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces
{
    public interface IGetEventByIdUseCase
    {
        public Task<EventDto> Execute(Guid id, CancellationToken cancellationToken);
    }
}
