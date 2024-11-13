using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Infrastructure.Services;

namespace EventsWebApplication.Application.UseCases.TokenUseCases
{
    public class UpdateRefreshTokenUseCase(IUnitOfWork unitOfWork, IRemoveRefreshTokenUseCase removeRefreshTokenUseCase,
        ITokenService tokenService, IGenerateJwtTokenUseCase generateJwtTokenUseCase, IMapper mapper) : IUpdateRefreshTokenUseCase
    {
        public async Task<string> Execute(Guid requestRefreshToken, string userAgent, CancellationToken cancellationToken)
        {
            var isValidRefreshToken = await tokenService.CheckRefreshToken(requestRefreshToken, cancellationToken);
            if (!isValidRefreshToken)
            {
                throw new KeyNotFoundException("Invalid or expired refresh token");
            }

            var user = mapper.Map<UserDto>(await unitOfWork.UserRepository.GetByRefreshToken(requestRefreshToken, cancellationToken));
            var jwt = await generateJwtTokenUseCase.Execute(user, cancellationToken);

            await removeRefreshTokenUseCase.Execute(requestRefreshToken, cancellationToken);
            await tokenService.GenerateRefreshToken(userAgent, user.Id, cancellationToken);

            return jwt;
        }
    }
}
