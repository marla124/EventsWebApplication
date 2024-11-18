using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class GetUserRoleUseCase(IUnitOfWork unitOfWork, IMapper mapper) : IGetUserRoleUseCase
    {
        public async Task<UserRoleDto> Execute(Guid userId, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetById(userId, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException(nameof(user));
            }
            var role = await unitOfWork.UserRoleRepository.GetById(user.UserRoleId, cancellationToken);
            return mapper.Map<UserRoleDto>(role);
        }
    }
}
