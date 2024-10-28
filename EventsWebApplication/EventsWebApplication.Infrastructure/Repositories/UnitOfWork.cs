using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventWebApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly ITokenRepository _tokenRepository;

        public UnitOfWork(EventWebApplicationDbContext dbContext, IEventRepository eventRepository, IUserRepository userRepository,
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
        public IEventRepository EventRepository => _eventRepository;
        public IRepository<UserRole> UserRoleRepository => _userRoleRepository;
        public IRepository<Category> CategoryRepository => _categoryRepository;
        public ITokenRepository TokenRepository => _tokenRepository;

    }
}
