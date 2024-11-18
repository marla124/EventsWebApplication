using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class GetEventParticipantByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper) : IGetEventParticipantByIdUseCase
    {
        public async Task<UserDto> Execute(Guid eventId, Guid userId, CancellationToken cancellationToken)
        {
            var userEventConnection = await unitOfWork.EventRepository.GetUserEventConnection(userId, eventId, cancellationToken);

            if (userEventConnection == null)
            {
                throw new KeyNotFoundException("User is not a participant of the event");
            }

            return mapper.Map<UserDto>(userEventConnection.User);
        }
    }
}