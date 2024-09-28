using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;

namespace EventsWebApplication.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventWebApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly ITokenRepository _tokenRepository;

        public UnitOfWork(EventWebApplicationDbContext dbContext, IRepository<Event> eventRepository, IUserRepository userRepository, 
            IRepository<UserRole> userRoleRepository, ITokenRepository tokenRepository, IRepository<Category> categoryRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _eventRepository = eventRepository;
            _tokenRepository = tokenRepository;
            _categoryRepository = categoryRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IRepository<Event> EventRepository => _eventRepository;
        public IRepository<UserRole> UserRoleRepository => _userRoleRepository;
        public IRepository<Category> CategoryRepository => _categoryRepository;
        public ITokenRepository TokenRepository => _tokenRepository;

    }
}
