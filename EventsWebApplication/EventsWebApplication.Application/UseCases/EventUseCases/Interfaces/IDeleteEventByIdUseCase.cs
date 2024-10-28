namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;

public interface IDeleteEventByIdUseCase
{
    public Task Execute(Guid id, Guid userId, CancellationToken cancellationToken);
}