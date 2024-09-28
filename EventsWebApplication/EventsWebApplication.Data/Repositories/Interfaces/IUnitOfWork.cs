using EventsWebApplication.Data.Entities;

namespace EventsWebApplication.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRepository<Event> EventRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<UserRole> UserRoleRepository { get; }
        ITokenRepository TokenRepository { get; }
    }
}
