using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Domain.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateJwtToken(User user, CancellationToken cancellationToken);
        public Task<Guid> GenerateRefreshToken(string userAgent,
            Guid userId, CancellationToken cancellationToken);
        public Task RemoveRefreshToken(Guid requestRefreshToken, CancellationToken cancellationToken);
        public Task<bool> CheckRefreshToken(Guid requestRefreshToken, CancellationToken cancellationToken);

    }
}
