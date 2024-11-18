using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class GetEventParticipantsUseCase(IUnitOfWork unitOfWork, IMapper mapper) : IGetEventParticipantsUseCase
    {
        public async Task<List<UserDto>?> Execute(Guid eventId, CancellationToken cancellationToken)
        {
            var eventExists = await unitOfWork.EventRepository.GetById(eventId, cancellationToken);
            if (eventExists == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            var users = await unitOfWork.ParticipantRepository.GetEventParticipants(eventId, cancellationToken);
            return mapper.Map<List<UserDto>?>(users);
        }
    }
}