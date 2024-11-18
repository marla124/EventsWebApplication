using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IEventRepository EventRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IRepository<UserRole> UserRoleRepository { get; }
        ITokenRepository TokenRepository { get; }
        IParticipantRepository ParticipantRepository { get; }
    }
}
