using AutoMapper;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases;

public class DeleteParticipantFromEventUseCase(IUnitOfWork unitOfWork) : IDeleteParticipantFromEventUseCase
{
    public async Task Execute(Guid userId, Guid eventId, CancellationToken cancellationToken)
    {
        var eventInfo = await unitOfWork.EventRepository.GetById(eventId, cancellationToken);
        if (eventInfo == null)
        {
            throw new KeyNotFoundException("Event not found");
        }
        var user = await unitOfWork.UserRepository.GetById(userId, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }
        var userEventConnection = await unitOfWork.EventRepository.GetUserEventConnection(userId, eventId, cancellationToken);

        if (userEventConnection == null)
        {
            throw new KeyNotFoundException("User is not a participant of the event");
        }

        unitOfWork.ParticipantRepository.DeleteParticipantFromEvent(userEventConnection);
        await unitOfWork.EventRepository.Commit(cancellationToken);
    }
}