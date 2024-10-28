using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.UserUseCases.Interface
{
    public interface IGetUserByEmailUseCase
    {
        Task<UserDto> Execute(string email, CancellationToken cancellationToken);

    }
}
