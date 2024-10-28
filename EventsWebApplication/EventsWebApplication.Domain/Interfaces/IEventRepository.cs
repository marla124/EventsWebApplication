using EventsWebApplication.Domain.Entities;
using System.Linq.Expressions;

namespace EventsWebApplication.Domain.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task AddParticipantToEvent(UserEventTime userEventTime, CancellationToken cancellationToken);
        Task<int> GetEventParticipantsCount(Guid eventId, CancellationToken cancellationToken);
        Task<UserEventTime?> GetUserEventConnection(Guid userId, Guid eventId, CancellationToken cancellationToken);
        void DeleteParticipantFromEvent(UserEventTime userEventTime);

        public Task<Event> GetByName(string name, CancellationToken cancellationToken,
            params Expression<Func<Event, object>>[] includes);
        Task<List<User>?> GetEventParticipants(Guid eventId, CancellationToken cancellationToken);

        Task<List<Event>?> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId,
            CancellationToken cancellationToken);

        Task<List<Event>?> GetUsersEvents(Guid userId, CancellationToken cancellationToken);

    }
}
