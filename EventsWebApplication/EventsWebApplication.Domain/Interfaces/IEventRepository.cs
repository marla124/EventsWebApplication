using EventsWebApplication.Domain.Entities;
using System.Linq.Expressions;

namespace EventsWebApplication.Domain.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<UserEventTime?> GetUserEventConnection(Guid userId, Guid eventId, CancellationToken cancellationToken);
        public Task<Event> GetByName(string name, CancellationToken cancellationToken,
            params Expression<Func<Event, object>>[] includes);

        Task<List<Event>?> GetEventsByCriteria(DateTime? date, string? address, Guid? categoryId,
            CancellationToken cancellationToken);

        Task<List<Event>?> GetUsersEvents(Guid userId, CancellationToken cancellationToken);

    }
}
