using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class GetUserByRefreshTokenUseCase(IUnitOfWork unitOfWork, IMapper mapper) : IGetUserByRefreshTokenUseCase
    {
        public async Task<UserDto> Execute(Guid refreshToken, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByRefreshToken(refreshToken, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return mapper.Map<UserDto>(user);
        }
    }
}