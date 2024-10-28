using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Domain.Interfaces;
using EventsWebApplication.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventsWebApplication.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly EventWebApplicationDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(EventWebApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
        {
            var resultQuery = _dbSet.AsQueryable();
            if (includes.Any())
            {
                resultQuery = includes.Aggregate(resultQuery, 
                    (current, include) => current.Include(include));
            }
            return await resultQuery.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
        }

        public virtual async Task DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var deleteEntity = await GetById(id, cancellationToken);
            if (deleteEntity != null)
            {
                _dbSet.Remove(deleteEntity);
            }
        }

        public virtual async Task<TEntity> CreateOne(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public IQueryable<TEntity> GetAsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            var resultQuery = _dbSet.AsQueryable().Where(expression);
            if (includes.Any())
            {
                resultQuery = includes.Aggregate(resultQuery, (current, include) => current.Include(include));
            }
            return resultQuery;
        }

        public async Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken)
        {
            _dbContext.Update(entity);
            return entity;
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
