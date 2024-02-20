using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        public Task<IQueryable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
