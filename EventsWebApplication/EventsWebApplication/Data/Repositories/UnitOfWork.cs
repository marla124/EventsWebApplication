using EventsWebApplication.Data.Entities;
using EventsWebApplication.Data.Repositories.Interfaces;

namespace EventsWebApplication.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventWebApplicationDbContext _dbContext;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<Category> _categoryRepository;

        public UnitOfWork(EventWebApplicationDbContext dbContext, IRepository<Event> eventRepository, IRepository<User> userRepository, IRepository<UserRole> userRoleRepository, IRepository<Category> categoryRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _categoryRepository = categoryRepository;
            _eventRepository = eventRepository;
        }

        public IRepository<User> UserRepository => _userRepository;
        public IRepository<Event> EventRepository => _eventRepository;
        public IRepository<UserRole> UserRoleRepository => _userRoleRepository;
        public IRepository<Category> CategoryRepository => _categoryRepository;

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
