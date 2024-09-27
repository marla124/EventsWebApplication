using EventsWebApplication.Data.Entities;

namespace EventsWebApplication.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<Event> EventRepository { get; }
        IRepository<UserRole> UserRoleRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        Task<int> Commit(CancellationToken cancellationToken);
    }
}
