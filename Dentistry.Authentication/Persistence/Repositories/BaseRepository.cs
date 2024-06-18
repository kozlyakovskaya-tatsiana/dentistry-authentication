using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected DentistryAuthenticationContext DentistryAuthenticationContext;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(DentistryAuthenticationContext dentistryAuthenticationContext)
        {
            DentistryAuthenticationContext = dentistryAuthenticationContext;
            DbSet = dentistryAuthenticationContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Query()
        {
            return DbSet.AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool isNoTracking = true)
        {
            
            var query = isNoTracking ? DbSet.AsNoTracking() : DbSet;
            
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

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToArrayAsync();
        }

        public void Create(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await DentistryAuthenticationContext.SaveChangesAsync();
        }

        /*public IQueryable<TEntity> GetAllWithInclude(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool disableTracking = true)
        {
            var query = disableTracking ? DbSet.AsNoTracking() : DbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }*/
    }
}