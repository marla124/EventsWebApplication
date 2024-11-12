using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.PasswordUseCases;
using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventsWebApplication.Application.UseCases.TokenUseCases
{
    public class GenerateJwtTokenUseCase(ITokenService tokenService, IMapper mapper) : IGenerateJwtTokenUseCase
    {
        public async Task<string> Execute(UserDto userDto, CancellationToken cancellationToken)
        {
            return await tokenService.GenerateJwtToken(mapper.Map<User>(userDto), cancellationToken);
        }
    }
}
