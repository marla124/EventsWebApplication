using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly EventWebApplicationDbContext _dbContext;

        public EventRepository(EventWebApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEventTime?> GetUserEventConnection(Guid userId, Guid eventId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserEventsTime
                .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId, cancellationToken);
        }

        public async Task<Event> GetByName(string name, CancellationToken cancellationToken, params Expression<Func<Event, object>>[] includes)
        {
            var resultQuery = _dbSet.AsQueryable();
            if (includes.Any())
            {
                resultQuery = includes.Aggregate(resultQuery,
                    (current, include)
                        => current.Include(include));
            }
            return await resultQuery.FirstOrDefaultAsync(entity => entity.Name.Equals(name), cancellationToken);
        }

        public async Task<List<Event>?> GetUsersEvents(Guid userId, CancellationToken cancellationToken)
        {
            var usersEvents = await _dbContext.UserEventsTime
                .Where(ev => ev.UserId == userId)
                .Include(ev => ev.Event)
                .ToListAsync(cancellationToken);

            var events = usersEvents.Select(ev => ev.Event).ToList();

            return events;
        }

        public async Task<List<Event>?> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId, CancellationToken cancellationToken)
        {
            var query = _dbSet.AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(e => e.DateAndTime.HasValue && e.DateAndTime.Value.Date == date.Value.Date);
            }

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(e => e.Address != null && e.Address.Contains(address));
            }

            if (categoryId.HasValue && categoryId.Value != Guid.Empty)
            {
                query = query.Where(e => e.CategoryId == categoryId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

    }
}
