namespace EventsWebApplication.Application.UseCases.UserUseCases.Interface
{
    public interface IDeleteUserByIdUseCase
    {
        Task Execute(Guid userId, CancellationToken cancellationToken);
    }
}
