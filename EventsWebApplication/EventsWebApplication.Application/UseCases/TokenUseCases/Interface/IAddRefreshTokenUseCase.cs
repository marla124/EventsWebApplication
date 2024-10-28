namespace EventsWebApplication.Application.UseCases.TokenUseCases.Interface
{
    public interface IAddRefreshTokenUseCase
    {
        public Task<Guid> Execute(string email, string userAgent, Guid userId, CancellationToken cancellationToken);

    }
}
