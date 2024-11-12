using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Infrastructure.Services;

namespace EventsWebApplication.Application.UseCases.TokenUseCases
{
    public class RemoveRefreshTokenUseCase(ITokenService tokenService) : IRemoveRefreshTokenUseCase
    {
        public async Task Execute(Guid requestRefreshToken, CancellationToken cancellationToken)
        {
            await tokenService.RemoveRefreshToken(requestRefreshToken, cancellationToken);
        }
    }
}