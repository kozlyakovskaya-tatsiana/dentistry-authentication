using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected DentistryAuthenticationContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(DentistryAuthenticationContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var query = DbSet.AsQueryable();
            
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToArrayAsync();
        }

        public async Task<TEntity?> GetAsync(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Create(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await DbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null)
                throw new KeyNotFoundException();

            DbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}