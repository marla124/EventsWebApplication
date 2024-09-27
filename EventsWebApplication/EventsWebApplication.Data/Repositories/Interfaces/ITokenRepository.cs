using EventsWebApplication.Data.Entities;

namespace EventsWebApplication.Data.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        public Task CreateRefreshToken(RefreshToken refToken, CancellationToken cancellationToken);
        public Task DeleteRefreshToken(Guid id, CancellationToken cancellationToken);
        public Task<RefreshToken> GetRefreshToken(Guid id, CancellationToken cancellationToken);
    }
}
