using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetAsync(Guid id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task SaveAsync();
    }
}
