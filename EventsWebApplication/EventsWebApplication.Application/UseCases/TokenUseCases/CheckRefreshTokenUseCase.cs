using EventsWebApplication.Application.UseCases.TokenUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

public class CheckRefreshTokenUseCase : ICheckRefreshTokenUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckRefreshTokenUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Execute(Guid requestRefreshToken, CancellationToken cancellationToken)
    {
        var token = await _unitOfWork.TokenRepository.GetRefreshToken(requestRefreshToken, cancellationToken);
        if (token == null)
        {
            return false;
        }
        return true;
    }
}