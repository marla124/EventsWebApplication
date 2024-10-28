namespace EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;

public interface IDeleteParticipantFromEventUseCase
{
    public Task Execute(Guid userId, Guid eventId, CancellationToken cancellationToken);
}