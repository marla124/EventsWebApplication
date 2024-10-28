using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases;

public class GetEventsByCriteriaUseCase :IGetEventsByCriteriaUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEventsByCriteriaUseCase(IMapper mapper, IUnitOfWork unitOfWork) 
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<List<EventDto>?> Execute(DateTime? date, string? address, Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var events = await _unitOfWork.EventRepository.GetEventsByCriteria(date, address, categoryId, cancellationToken);
        return _mapper.Map<List<EventDto>>(events);
    }
}