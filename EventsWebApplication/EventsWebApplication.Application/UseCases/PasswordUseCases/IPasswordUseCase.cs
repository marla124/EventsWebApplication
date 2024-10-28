namespace EventsWebApplication.Application.UseCases.PasswordUseCases
{
    public interface IPasswordUseCase
    {
        Task<bool> CheckPasswordCorrect(string email, string password, CancellationToken cancellationToken);
        string MdHashGenerate(string input);
    }
}
