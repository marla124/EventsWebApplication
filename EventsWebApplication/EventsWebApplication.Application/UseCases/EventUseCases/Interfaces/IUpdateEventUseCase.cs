using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;

public interface IUpdateEventUseCase
{
    public Task<UpdateEventDto> Execute(UpdateEventDto dto, Guid userId, CancellationToken cancellationToken);
}