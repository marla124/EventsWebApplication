using EventsWebApplication.Application.Dto;

namespace EventsWebApplication.Application.UseCases.UserUseCases.Interface
{
    public interface IGetUserRoleUseCase
    {
        public Task<UserRoleDto> Execute(Guid userId, CancellationToken cancellationToken);
    }
}
