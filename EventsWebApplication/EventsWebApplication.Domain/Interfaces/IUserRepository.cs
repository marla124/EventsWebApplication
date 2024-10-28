using EventsWebApplication.Domain.Entities;
using System.Linq.Expressions;

namespace EventsWebApplication.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetByEmail(string email, CancellationToken cancellationToken,
            params Expression<Func<User, object>>[] includes);

        public Task<User> GetByRefreshToken(Guid refreshToken, CancellationToken cancellationToken);
    }
}
