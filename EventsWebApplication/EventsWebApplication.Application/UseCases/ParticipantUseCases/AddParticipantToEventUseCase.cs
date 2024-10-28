using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases
{
    public class AddParticipantToEventUseCase : IAddParticipantToEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddParticipantToEventUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(Guid userId, Guid eventId, CancellationToken cancellationToken)
        {
            var userEvent = await _unitOfWork.EventRepository.GetUserEventConnection(userId, eventId, cancellationToken);
            if (userEvent != null)
            {
                throw new InvalidOperationException("User is already a participant of the event");
            }

            var eventInfo = await _unitOfWork.EventRepository.GetById(eventId, cancellationToken);
            if (eventInfo == null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            var maxNumber = eventInfo.MaxNumberOfPeople;
            var numberOfPeopleNow = await _unitOfWork.EventRepository.GetEventParticipantsCount(eventId, cancellationToken);

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

            await _unitOfWork.EventRepository.AddParticipantToEvent(userEventTime, cancellationToken);
            await _unitOfWork.EventRepository.Commit(cancellationToken);
        }
    }
}