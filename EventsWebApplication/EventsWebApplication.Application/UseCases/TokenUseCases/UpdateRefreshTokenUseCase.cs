using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.TokenUseCases
{
    public class UpdateRefreshTokenUseCase(IUnitOfWork unitOfWork, ITokenService tokenService) : IUpdateRefreshTokenUseCase
    {
        public async Task<string> Execute(Guid requestRefreshToken, string userAgent, CancellationToken cancellationToken)
        {
            var isValidRefreshToken = await tokenService.CheckRefreshToken(requestRefreshToken, cancellationToken);
            if (!isValidRefreshToken)
            {
                throw new KeyNotFoundException("Invalid or expired refresh token");
            }

            var user = await unitOfWork.UserRepository.GetByRefreshToken(requestRefreshToken, cancellationToken);
            var jwt = await tokenService.GenerateJwtToken(user, cancellationToken);

            await tokenService.RemoveRefreshToken(requestRefreshToken, cancellationToken);
            await tokenService.GenerateRefreshToken(userAgent, user.Id, cancellationToken);

            return jwt;
        }
    }
}
