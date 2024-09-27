using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static EventsWebApplication.Data.Repositories.TokenRepository;

namespace EventsWebApplication.Data.Repositories
{
    public class TokenRepository: ITokenRepository
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
        public async Task DeleteRefreshToken(Guid id, CancellationToken cancellationToken) 
        {
            var delEntity = await GetRefreshToken(id, cancellationToken: cancellationToken);
            if (delEntity != null)
            {
                _dbContext.RefreshTokens.Remove(delEntity);
            }
            else
            { 
                throw new ArgumentException("Incorrect id for delete", nameof(id));
            }
        }

        public async Task<RefreshToken> GetRefreshToken(Guid id, CancellationToken cancellationToken) 
        { 
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken: cancellationToken) 
                   ?? throw new InvalidOperationException();
        }
    }
}

