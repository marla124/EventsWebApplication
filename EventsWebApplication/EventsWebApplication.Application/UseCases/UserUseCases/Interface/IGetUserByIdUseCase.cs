using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.UserUseCases.Interface
{
    public interface IGetUserByIdUseCase
    {
        public Task<UserDto> Execute(Guid id, CancellationToken cancellationToken);
    }
}
