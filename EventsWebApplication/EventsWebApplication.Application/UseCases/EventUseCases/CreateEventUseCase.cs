using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases;

public class CreateEventUseCase : ICreateEventUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateEventUseCase(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<EventDto> Execute(EventDto dto, CancellationToken cancellationToken)
    {
        var eventInfo = new Event()
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            DateAndTime = dto.DateAndTime,
            CategoryId = dto.CategoryId,
            UserCreatorId = dto.UserCreatorId,
            MaxNumberOfPeople = dto.MaxNumberOfPeople,
            Address = dto.Address,
            Image = dto.Image,
            CreatedAt = DateTime.UtcNow,
        };
        var createdEntity = await _unitOfWork.EventRepository.CreateOne(eventInfo, cancellationToken);
        await _unitOfWork.EventRepository.Commit(cancellationToken);
        return _mapper.Map<EventDto>(createdEntity);
    }
}