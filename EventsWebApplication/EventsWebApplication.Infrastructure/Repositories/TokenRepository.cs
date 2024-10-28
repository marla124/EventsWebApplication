using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static EventsWebApplication.Infrastructure.Repositories.TokenRepository;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly EventWebApplicationDbContext _dbContext;

        public TokenRepository(EventWebApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateRefreshToken(RefreshToken refToken, CancellationToken cancellationToken)
        {
            await _dbContext.RefreshTokens.AddAsync(refToken, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteRefreshToken(RefreshToken refToken, CancellationToken cancellationToken)
        {
            _dbContext.RefreshTokens.Remove(refToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken> GetRefreshToken(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken: cancellationToken);
        }
    }
}

