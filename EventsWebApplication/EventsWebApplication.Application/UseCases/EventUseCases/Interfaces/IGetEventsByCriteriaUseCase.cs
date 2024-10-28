using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;

public interface IGetEventsByCriteriaUseCase
{
    public Task<List<EventDto>?> Execute(DateTime? date, string? address, Guid? categoryId,
        CancellationToken cancellationToken);

}