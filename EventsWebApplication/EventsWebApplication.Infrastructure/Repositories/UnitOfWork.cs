using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class UnitOfWork(EventWebApplicationDbContext dbContext, IEventRepository eventRepository, IUserRepository userRepository,
        IRepository<UserRole> userRoleRepository, ITokenRepository tokenRepository, ICategoryRepository categoryRepository, 
        IParticipantRepository participantRepository) : IUnitOfWork
    {

        public IUserRepository UserRepository => userRepository;
        public IEventRepository EventRepository => eventRepository;
        public IRepository<UserRole> UserRoleRepository => userRoleRepository;
        public ICategoryRepository CategoryRepository => categoryRepository;
        public ITokenRepository TokenRepository => tokenRepository;
        public IParticipantRepository ParticipantRepository => participantRepository;

    }
}
