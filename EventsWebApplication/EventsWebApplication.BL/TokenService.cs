using AutoMapper;
using EventsWebApplication.BL.Dto;
using EventsWebApplication.BL.Interfaces;
using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UAParser;

namespace EventsWebApplication.BL
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public TokenService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
        }
        public async Task<Guid> AddRefreshToken(Guid id, string userAgent, Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetById(id, cancellationToken);
            if (user != null)
            {
                var refTokenDto = new RefreshTokenDto
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    ExpiringAt = DateTime.UtcNow.AddDays(5),
                    AssociateDeviceName = GetDeviceName(userAgent),
                    IsActive = true,
                    UserId = userId,
                };
                var refToken = _mapper.Map<RefreshToken>(refTokenDto);
                await _unitOfWork.TokenRepository.CreateRefreshToken(refToken, cancellationToken);
                return refToken.Id;
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        private string GetDeviceName(string userAgent)
        {
            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(userAgent);
            string deviceName = clientInfo.Device.ToString();
            return deviceName;
        }
        public async Task<string> GenerateJwtToken(UserDto userDto, CancellationToken cancellationToken)
        {
            var isLifetime = int.TryParse(_configuration["Jwt:Lifetime"], out var lifetime);
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            var iss = _configuration["Jwt:Issuer"];
            var aud = _configuration["Jwt:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Email, userDto.Email),
                        new Claim(ClaimTypes.Role, "Admin"),
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

        public async Task<bool> CheckRefreshToken(Guid requestRefreshToken, CancellationToken cancellationToken)
        {
            var token = await _unitOfWork.TokenRepository.GetRefreshToken(requestRefreshToken, cancellationToken);
            if (token == null)
            {
                return false;
            }
            return true;
        }

        public async Task RemoveRefreshToken(Guid requestRefreshToken, CancellationToken cancellationToken)
        {
            await _unitOfWork.TokenRepository.DeleteRefreshToken(requestRefreshToken, cancellationToken);
        }
    }
}
