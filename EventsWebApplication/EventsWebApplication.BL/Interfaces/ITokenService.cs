using EventsWebApplication.BL.Dto;

namespace EventsWebApplication.BL.Interfaces
{
    public interface ITokenService
    {
        Task<Guid> AddRefreshToken(Guid id, string userAgent, Guid userId, CancellationToken cancellationToken);
        Task<string> GenerateJwtToken(UserDto userDto, CancellationToken cancellationToken);
        Task<bool> CheckRefreshToken(Guid requestRefreshToken, CancellationToken cancellationToken);
        Task RemoveRefreshToken(Guid requestRefreshToken, CancellationToken cancellationToken);
    }
}
