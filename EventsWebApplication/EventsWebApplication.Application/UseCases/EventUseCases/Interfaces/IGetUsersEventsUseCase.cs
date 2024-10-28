using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces
{
    public interface IGetUsersEventsUseCase
    {
        public Task<List<EventDto>?> Execute(Guid userId, CancellationToken cancellationToken);

    }
}
