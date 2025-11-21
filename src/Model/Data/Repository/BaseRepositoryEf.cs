using Microsoft.EntityFrameworkCore;

namespace Model.Data.Repository;

public abstract class BaseRepositoryEf<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : class
{
    protected BaseRepositoryEf(TDbContext  dbContext)
    {
        Context = dbContext;
        DbSet  = Context.Set<TEntity>();
    }
    
    private TDbContext Context { get; }
    
    private DbSet<TEntity> DbSet { get; }
    
    protected virtual IQueryable<TEntity> Query => DbSet;

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        var entityChanged = await Context.SaveChangesAsync();
        
        return entityChanged > 0;
    }
}