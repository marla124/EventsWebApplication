using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.UserUseCases.Interface
{
    public interface IGetUserByRefreshTokenUseCase
    {
        Task<UserDto> Execute(Guid refreshToken, CancellationToken cancellationToken);
    }
}
