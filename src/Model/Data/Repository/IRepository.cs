namespace Model.Data.Repository;

public interface IRepository<TEntity>
{
    public Task<TEntity> CreateAsync(TEntity entity);
    public Task<TEntity> UpdateAsync(TEntity entity);

    public Task<bool> DeleteAsync(TEntity entity);
}