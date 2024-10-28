using AutoMapper;
using EventsWebApplication.Application.Dto;
using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Application.UseCases.UserUseCases.Interface;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using UAParser;

namespace EventsWebApplication.Application.UseCases.TokenUseCases
{
    public class AddRefreshTokenUseCase : IAddRefreshTokenUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGetUserByEmailUseCase _getUserByEmailUseCase;
        public AddRefreshTokenUseCase(IUnitOfWork unitOfWork, IMapper mapper, IGetUserByEmailUseCase getUserByEmailUseCase)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _getUserByEmailUseCase = getUserByEmailUseCase;
        }
        public async Task<Guid> Execute(string email, string userAgent, Guid userId, CancellationToken cancellationToken)
        {
            var user = await _getUserByEmailUseCase.Execute(email, cancellationToken);
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
                throw new KeyNotFoundException("User not found");
            }
        }

        private string GetDeviceName(string userAgent)
        {
            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(userAgent);
            string deviceName = clientInfo.Device.ToString();
            return deviceName;
        }
    }
}
