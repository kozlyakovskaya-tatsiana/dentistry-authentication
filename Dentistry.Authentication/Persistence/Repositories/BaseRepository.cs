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

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DentistryAuthenticationContext.Set<TEntity>().ToArrayAsync();
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
    }
}