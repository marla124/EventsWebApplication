using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly EventWebApplicationDbContext _dbContext;

        public ParticipantRepository(EventWebApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddParticipantToEvent(UserEventTime userEventTime, CancellationToken cancellationToken)
        {
            await _dbContext.UserEventsTime.AddAsync(userEventTime, cancellationToken);
        }

        public async Task<int> GetEventParticipantsCount(Guid eventId, CancellationToken cancellationToken)
        {
            return await _dbContext.UserEventsTime
                .CountAsync(ue => ue.EventId == eventId, cancellationToken);
        }

        public void DeleteParticipantFromEvent(UserEventTime userEventTime)
        {
            _dbContext.UserEventsTime.Remove(userEventTime);
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
    }
}
