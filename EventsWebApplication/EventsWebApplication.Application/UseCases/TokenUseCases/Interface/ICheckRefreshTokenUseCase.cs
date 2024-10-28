namespace EventsWebApplication.Application.UseCases.TokenUseCases.Interface
{
    public interface ICheckRefreshTokenUseCase
    {
        public Task<bool> Execute(Guid requestRefreshToken, CancellationToken cancellationToken);

    }
}
