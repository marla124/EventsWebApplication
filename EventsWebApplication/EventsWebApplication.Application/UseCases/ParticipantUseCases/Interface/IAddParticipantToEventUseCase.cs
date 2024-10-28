namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;

public interface IAddParticipantToEventUseCase
{
    public Task Execute(Guid userId, Guid eventId, CancellationToken cancellationToken);

}