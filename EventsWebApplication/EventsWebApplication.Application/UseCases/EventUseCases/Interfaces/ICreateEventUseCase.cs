using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;

public interface ICreateEventUseCase
{
    public Task<EventDto> Execute(EventDto dto, CancellationToken cancellationToken);
}