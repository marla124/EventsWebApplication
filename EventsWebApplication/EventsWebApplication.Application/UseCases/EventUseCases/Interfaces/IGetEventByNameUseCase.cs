using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;

public interface IGetEventByNameUseCase
{
    public Task<EventDto> Execute(string name, CancellationToken cancellationToken);

}