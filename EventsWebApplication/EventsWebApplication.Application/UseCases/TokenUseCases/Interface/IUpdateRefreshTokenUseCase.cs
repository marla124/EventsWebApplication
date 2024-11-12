namespace EventsWebApplication.Application.UseCases.TokenUseCases.Interface
{
    public interface IUpdateRefreshTokenUseCase
    {
        public Task<string> Execute(Guid requestRefreshToken, string userAgent, CancellationToken cancellationToken);
    }
}
