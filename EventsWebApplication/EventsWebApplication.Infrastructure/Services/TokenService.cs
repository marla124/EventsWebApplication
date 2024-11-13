using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UAParser;

namespace EventsWebApplication.Infrastructure.Services
{
    public class TokenService(IConfiguration configuration, IUnitOfWork unitOfWork) : ITokenService
    {
        public async Task<string> GenerateJwtToken(User user, CancellationToken cancellationToken)
        {
            var role = await unitOfWork.UserRoleRepository.GetById(user.UserRoleId, cancellationToken);
            var isLifetime = int.TryParse(configuration["Jwt:Lifetime"], out var lifetime);
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]);
            var iss = configuration["Jwt:Issuer"];
            var aud = configuration["Jwt:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, role.Role),
                    new Claim("aud",aud),
                    new Claim("iss",iss),
                    new Claim("userId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(lifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<Guid> GenerateRefreshToken(string userAgent, Guid userId, CancellationToken cancellationToken)
        {
            var refToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ExpiringAt = DateTime.UtcNow.AddDays(5),
                AssociateDeviceName = GetDeviceName(userAgent),
                IsActive = true,
                UserId = userId,
            };
            await unitOfWork.TokenRepository.CreateRefreshToken(refToken, cancellationToken);
            return refToken.Id;
        }

        public async Task RemoveRefreshToken(Guid requestRefreshToken, CancellationToken cancellationToken)
        {
            var refreshToken = await unitOfWork.TokenRepository.GetRefreshToken(requestRefreshToken, cancellationToken);

            if (refreshToken == null)
            {
                throw new KeyNotFoundException("Refresh token not found");
            }

            await unitOfWork.TokenRepository.DeleteRefreshToken(refreshToken, cancellationToken);
        }

        private string GetDeviceName(string userAgent)
        {
            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(userAgent);
            string deviceName = clientInfo.Device.ToString();
            return deviceName;
        }

        public async Task<bool> CheckRefreshToken(Guid requestRefreshToken, CancellationToken cancellationToken)
        {
            var token = await unitOfWork.TokenRepository.GetRefreshToken(requestRefreshToken, cancellationToken);
            if (token == null)
            {
                return false;
            }
            return true;
        }
    }
}
