using System.Linq.Expressions;
using EventsWebApplication.Data.Entities;

namespace EventsWebApplication.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetById(Guid id,CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes);
        Task DeleteById(Guid id, CancellationToken cancellationToken);
        Task CreateOne(TEntity entity, CancellationToken cancellationToken);
        IQueryable<TEntity> GetAsQueryable();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);
        Task<List<TEntity>> FindBy(CancellationToken cancellationToken);
        Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken);
    }
}