using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.UserUseCases.Interface
{
    public interface IGetUsersUseCase
    {
        public Task<UserDto[]> Execute(CancellationToken cancellationToken);
    }
}
