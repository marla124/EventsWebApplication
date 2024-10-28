using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventsWebApplication.Infrastructure;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly EventWebApplicationDbContext _dbContext;

        public UserRepository(EventWebApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByEmail(string email, CancellationToken cancellationToken,
            params Expression<Func<User, object>>[] includes)
        {
            var resultQuery = _dbSet.AsQueryable();
            if (includes.Any())
            {
                resultQuery = includes.Aggregate(resultQuery,
                    (current, include)
                        => current.Include(include));
            }
            return await resultQuery.FirstOrDefaultAsync(entity => entity.Email.Equals(email), cancellationToken);
        }

        public async Task<User> GetByRefreshToken(Guid refreshToken, CancellationToken cancellationToken)
        {
            var resultQuery = _dbSet.AsQueryable();
            return await resultQuery.FirstOrDefaultAsync(entity => entity.RefreshTokens.Any(rt => rt.Id == refreshToken), cancellationToken);
        }
    }
}
