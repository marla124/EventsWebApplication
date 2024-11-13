using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.AuthUseCases
{
    public interface IRegisterUserUseCase
    {
        Task<UserDto> Execute(UserDto dto, CancellationToken cancellationToken);
    }
}
