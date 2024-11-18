using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class GetUserByEmailUseCase(IUnitOfWork unitOfWork, IMapper mapper) : IGetUserByEmailUseCase
    {
        public async Task<UserDto> Execute(string email, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByEmail(email, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return mapper.Map<UserDto>(user);
        }
    }
}