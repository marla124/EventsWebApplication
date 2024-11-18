using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EventWebApplicationDbContext _dbContext;

        public CategoryRepository(EventWebApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category[]> GetCategories(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Category>().ToArrayAsync(cancellationToken);
        }
    }
}
