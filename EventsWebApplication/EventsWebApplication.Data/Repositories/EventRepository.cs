using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventsWebApplication.Data.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly EventWebApplicationDbContext _dbContext;

        public EventRepository(EventWebApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddParticipantToEvent(Guid userId, Guid eventId, CancellationToken cancellationToken)
        {
            var userEventExists = await _dbContext.UserEventsTime
                .AnyAsync(ue => ue.UserId == userId && ue.EventId == eventId, cancellationToken);

<<<<<<< Updated upstream
=======
            var numberOfPepleNow = await _dbContext.UserEventsTime.CountAsync(cancellationToken);
            var eventInfo = await GetById(eventId, cancellationToken);
            var maxNumber = eventInfo.MaxNumberOfPeople;
            if (numberOfPepleNow > maxNumber || userEventExists)
            {
                throw new InvalidOperationException();
            }

>>>>>>> Stashed changes
            var userEvent = new UserEventTime()
            {
                UserId = userId,
                EventId = eventId,
                RegistrationDate = DateTime.UtcNow
            };

            await _dbContext.UserEventsTime.AddAsync(userEvent, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }


        public async Task DeleteParticipantFromEvent(Guid userId, Guid eventId, CancellationToken cancellationToken)
        {
            var userEvent = await _dbContext.UserEventsTime
                .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId, cancellationToken);

            if (userEvent != null)
            {
                _dbContext.UserEventsTime.Remove(userEvent);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new KeyNotFoundException("User is not a participant of the event");
            }
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

        public async Task<User> GetEventParticipantById(Guid eventId, Guid userId, CancellationToken cancellationToken)
        {
            var userEvent = await _dbContext.UserEventsTime
                .Include(ue => ue.User)
                .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId, cancellationToken);

            return userEvent.User;
        }

        public async Task<List<User>?> GetEventParticipants(Guid eventId, CancellationToken cancellationToken)
        {
            var userEvents = await _dbContext.UserEventsTime
                .Where(ue => ue.EventId == eventId)
                .Include(ue => ue.User)
                .ToListAsync(cancellationToken);

            var users = userEvents.Select(ue => ue.User).ToList();

            return users;
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

            if (categoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == categoryId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

    }
}
