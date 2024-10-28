using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Domain.Interfaces
{
    public interface ITokenRepository
    {
        public Task CreateRefreshToken(RefreshToken refToken, CancellationToken cancellationToken);
        public Task DeleteRefreshToken(RefreshToken refToken, CancellationToken cancellationToken);
        public Task<RefreshToken> GetRefreshToken(Guid id, CancellationToken cancellationToken);
    }
}
