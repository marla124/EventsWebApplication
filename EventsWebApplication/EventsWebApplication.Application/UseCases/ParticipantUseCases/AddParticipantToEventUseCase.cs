using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class AddParticipantToEventUseCase(IUnitOfWork unitOfWork) : IAddParticipantToEventUseCase
    {
        public async Task Execute(Guid userId, Guid eventId, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            var userEvent = await unitOfWork.EventRepository.GetUserEventConnection(userId, eventId, cancellationToken);
            if (userEvent != null)
            {
                throw new InvalidOperationException("User is already a participant of the event");
            }

            var eventInfo = await unitOfWork.EventRepository.GetById(eventId, cancellationToken);
            if (eventInfo == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            var maxNumber = eventInfo.MaxNumberOfPeople;
            var numberOfPeopleNow = await unitOfWork.ParticipantRepository.GetEventParticipantsCount(eventId, cancellationToken);

            if (numberOfPeopleNow >= maxNumber)
            {
                throw new InvalidOperationException("Event is full");
            }

            var userEventTime = new UserEventTime
            {
                UserId = userId,
                EventId = eventId,
                RegistrationDate = DateTime.UtcNow
            };

            await unitOfWork.ParticipantRepository.AddParticipantToEvent(userEventTime, cancellationToken);
            await unitOfWork.EventRepository.Commit(cancellationToken);
        }
    }
}