using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Query();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool isNoTracking = true);
        Task<TEntity?> GetAsync(Guid id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task SaveAsync();
    }
}