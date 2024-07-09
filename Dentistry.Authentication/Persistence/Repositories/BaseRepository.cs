using System.Linq.Expressions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

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

    public async Task CreateAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }
    // pass id?
    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FirstOrDefaultAsync(e => e.Id == id);
        if (entity is null)
            throw new EntityNotFoundException($"Entity {nameof(TEntity)} with id = ${id} wasn't found.");

        DbSet.Remove(entity);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}