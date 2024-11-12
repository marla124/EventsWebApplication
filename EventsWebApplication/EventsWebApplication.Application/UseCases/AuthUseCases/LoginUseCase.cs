using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Infrastructure.Services;
using AutoMapper;
using EventsWebApplication.Application.UseCases.AuthUseCases;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class LoginUseCase(IUnitOfWork unitOfWork, IMapper mapper, 
        ITokenService tokenService, IGetUserByEmailUseCase getUserByEmailUseCase) : ILoginUseCase
    {
        public async Task<string> Execute(string email, string userAgent, CancellationToken cancellationToken)
        {
            var userDto = await getUserByEmailUseCase.Execute(email, cancellationToken);
            if (userDto == null) 
            {
                throw new ArgumentNullException(nameof(userDto));
            }
            var jwtToken = await tokenService.GenerateJwtToken(mapper.Map<User>(userDto), cancellationToken);
            var refreshToken = await tokenService.GenerateRefreshToken(userAgent, userDto.Id, cancellationToken);
            return jwtToken;
        }
    }
}
