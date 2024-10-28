namespace EventsWebApplication.Application.UseCases.TokenUseCases.Interface
{
    public interface IRemoveRefreshTokenUseCase
    {
        public Task Execute(Guid requestRefreshToken, CancellationToken cancellationToken);

    }
}
