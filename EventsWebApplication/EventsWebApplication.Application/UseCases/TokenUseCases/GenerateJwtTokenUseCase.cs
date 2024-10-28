using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.PasswordUseCases;
using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventsWebApplication.Application.UseCases.TokenUseCases
{
    public class GenerateJwtTokenUseCase : IGenerateJwtTokenUseCase
    {
        private readonly IConfiguration _configuration;
        private readonly IGetUserRoleUseCase _getUserRoleUseCase;
        private readonly IPasswordUseCase _passwordUseCase;
        public GenerateJwtTokenUseCase(IConfiguration configuration, IGetUserRoleUseCase getUserRoleUseCase, IPasswordUseCase passwordUseCase)
        {
            _configuration = configuration;
            _getUserRoleUseCase = getUserRoleUseCase;
            _passwordUseCase = passwordUseCase;
        }
        public async Task<string> Execute(UserDto userDto, CancellationToken cancellationToken)
        {
            var role = (await _getUserRoleUseCase.Execute(userDto.Id, cancellationToken)).Role;
            var isLifetime = int.TryParse(_configuration["Jwt:Lifetime"], out var lifetime);
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            var iss = _configuration["Jwt:Issuer"];
            var aud = _configuration["Jwt:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, userDto.Email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("aud",aud),
                    new Claim("iss",iss),
                    new Claim("userId", userDto.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(lifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
