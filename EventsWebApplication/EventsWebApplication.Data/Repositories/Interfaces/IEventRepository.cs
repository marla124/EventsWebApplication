using EventsWebApplication.Data.Entities;
using System.Linq.Expressions;

namespace EventsWebApplication.Data.Repositories.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task AddParticipantToEvent(Guid userId, Guid eventId, CancellationToken cancellationToken);
        Task DeleteParticipantFromEvent(Guid userId, Guid eventId, CancellationToken cancellationToken);
        public Task<Event> GetByName(string name, CancellationToken cancellationToken,
            params Expression<Func<Event, object>>[] includes);
        Task<User> GetEventParticipantById(Guid eventId, Guid userId, CancellationToken cancellationToken);
        Task<List<User>?> GetEventParticipants(Guid eventId, CancellationToken cancellationToken);

        Task<List<Event>?> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId,
            CancellationToken cancellationToken);

        Task<List<Event>?> GetUsersEvents(Guid userId, CancellationToken cancellationToken);

    }
}
