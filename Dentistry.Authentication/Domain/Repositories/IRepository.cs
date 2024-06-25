using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<TEntity?> GetAsync(Guid id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(Guid id);
        Task SaveAsync();
    }
}