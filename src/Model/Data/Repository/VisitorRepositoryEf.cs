using Microsoft.EntityFrameworkCore;
using Model.Domain;

namespace Model.Data.Repository;

public sealed class VisitorRepositoryEf(DatabaseContext dbContext)
    : BaseRepositoryEf<DatabaseContext, Visitor>(dbContext), IVisitorRepository
{
    public Task<Visitor?> TryGetAsync(int id)
    {
        return Query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deletedRows = await Query.Where(x => x.Id == id).ExecuteDeleteAsync();
        return deletedRows > 0;
    }

    public async Task<IReadOnlyCollection<Visitor>> GetAllAsync()
    {
        return await Query.AsNoTracking().ToListAsync();
    }
}