using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.TokenUseCases
{
    public class RemoveRefreshTokenUseCase : IRemoveRefreshTokenUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRefreshTokenUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(Guid requestRefreshToken, CancellationToken cancellationToken)
        {
            var refreshToken = await _unitOfWork.TokenRepository.GetRefreshToken(requestRefreshToken, cancellationToken);

            if (refreshToken == null)
            {
                throw new KeyNotFoundException("Refresh token not found");
            }

            await _unitOfWork.TokenRepository.DeleteRefreshToken(refreshToken, cancellationToken);
        }
    }
}