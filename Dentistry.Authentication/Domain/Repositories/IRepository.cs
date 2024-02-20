using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(int id);
        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
    }
}
