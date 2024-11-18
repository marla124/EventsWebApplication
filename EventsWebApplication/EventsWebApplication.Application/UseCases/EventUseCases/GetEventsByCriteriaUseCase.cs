using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases;

public class GetEventsByCriteriaUseCase(IUnitOfWork unitOfWork, IMapper mapper) :IGetEventsByCriteriaUseCase
{
    public async Task<List<EventDto>?> Execute(DateTime? date, string? address, Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var events = await unitOfWork.EventRepository.GetEventsByCriteria(date, address, categoryId, cancellationToken);
        return mapper.Map<List<EventDto>>(events);
    }
}