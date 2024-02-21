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

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DentistryAuthenticationContext.Set<TEntity>().ToArrayAsync();
        }

        public async Task<TEntity?> GetAsync(Guid id)
        {
            return await DbSet.FindAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToArrayAsync();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await DentistryAuthenticationContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await DentistryAuthenticationContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await DentistryAuthenticationContext.SaveChangesAsync();
        }
    }
}
