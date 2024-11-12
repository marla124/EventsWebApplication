namespace EventsWebApplication.Application.UseCases.EventUseCases.Interfaces;

public interface IDeleteEventByIdUseCase
{
    public Task Execute(Guid id, CancellationToken cancellationToken);
}