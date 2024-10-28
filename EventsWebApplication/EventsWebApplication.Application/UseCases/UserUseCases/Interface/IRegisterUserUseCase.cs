using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.UserUseCases.Interface
{
    public interface IRegisterUserUseCase
    {
        Task<UserDto> Execute(UserDto dto, CancellationToken cancellationToken);
    }
}
