namespace EventsWebApplication.Application.UseCases.PasswordUseCases
{
    public interface ICheckPasswordUseCase
    {
        Task<bool> CheckPasswordCorrect(string email, string password, CancellationToken cancellationToken);
    }
}
