using System.Data;
using Dapper;
using Model.Domain;

namespace Model.Data.Repository;

public class VisitorRepositoryDapper(IDbConnection dbConnection) : IVisitorRepository
{
    public async Task<Visitor> CreateAsync(Visitor entity)
    {
        return await dbConnection.QueryFirstAsync<Visitor>(QueryStore.InsertVisitorQuery, entity);
    }

    public async Task<Visitor> UpdateAsync(Visitor entity)
    {
        return await dbConnection.QueryFirstAsync<Visitor>(QueryStore.UpdateVisitorById, entity);
    }

    public async Task<bool> DeleteAsync(Visitor entity)
    {
        return await dbConnection.ExecuteAsync(QueryStore.DeleteVisitorById, entity) > 0;
    }

    public async Task<Visitor?> TryGetAsync(int id)
    {
        return await dbConnection.QueryFirstOrDefaultAsync<Visitor>(QueryStore.SelectVisitorById, new { Id = id });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await dbConnection.ExecuteAsync(QueryStore.DeleteVisitorById, new { Id = id }) > 0;
    }

    public async Task<IReadOnlyCollection<Visitor>> GetAllAsync(int limit)
    {
        var visitors = await dbConnection
            .QueryAsync<Visitor>(QueryStore.SelectAllVisitorsWithLimit, new { Limit = limit });
        
        return visitors.ToList();
    }

    public async Task<IReadOnlyCollection<Visitor>> GetSinceIdAsync(int sinceId, int limit)
    {
        var visitors = await dbConnection
            .QueryAsync<Visitor>(QueryStore.SelectAllVisitorsSinceIdWithLimit,
                new { Limit = limit, SinceId = sinceId });
        
        return visitors.ToList();
    }
}