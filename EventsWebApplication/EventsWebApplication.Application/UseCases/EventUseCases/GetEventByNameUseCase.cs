using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.EventUseCases
{
    public class GetEventByNameUseCase(IUnitOfWork unitOfWork, IMapper mapper) : IGetEventByNameUseCase
    {
        public async Task<EventDto> Execute(string name, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.EventRepository.GetByName(name, cancellationToken);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            return mapper.Map<EventDto>(eventEntity);
        }
    }
}
