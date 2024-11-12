using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class GetUserByIdUseCase(IMapper mapper, IUnitOfWork unitOfWork) : IGetUserByIdUseCase
    {
        public async Task<UserDto> Execute(Guid id, CancellationToken cancellationToken)
        {
            var entity = await unitOfWork.UserRepository.GetById(id, cancellationToken);

            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            var dto = mapper.Map<UserDto>(entity);
            return dto;
        }
    }
}
