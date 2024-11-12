using System.Linq.Expressions;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes);
        Task DeleteById(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> CreateOne(TEntity entity, CancellationToken cancellationToken);
        IQueryable<TEntity> GetAsQueryable();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken);
        Task Commit(CancellationToken cancellationToken);
    }
}