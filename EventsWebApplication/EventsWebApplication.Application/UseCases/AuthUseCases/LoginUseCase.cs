using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Domain.Entities;
using AutoMapper;
using EventsWebApplication.Application.UseCases.AuthUseCases;

namespace EventsWebApplication.Application.UseCases.UserUseCases
{
    public class LoginUseCase(IUnitOfWork unitOfWork, ITokenService tokenService) : ILoginUseCase
    {
        public async Task<string> Execute(string email, string userAgent, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByEmail(email, cancellationToken);
            if (user == null) 
            {
                throw new ArgumentNullException(nameof(user));
            }
            var jwtToken = await tokenService.GenerateJwtToken(user, cancellationToken);
            var refreshToken = await tokenService.GenerateRefreshToken(userAgent, user.Id, cancellationToken);
            return jwtToken;
        }
    }
}
