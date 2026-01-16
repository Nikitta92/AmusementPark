using Model.Domain;

namespace Model.Data.Repository;

public interface IVisitorRepository : IRepository<Visitor>
{
    Task<Visitor?> TryGetAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<IReadOnlyCollection<Visitor>> GetAllAsync(int limit);
    Task<IReadOnlyCollection<Visitor>> GetSinceIdAsync(int sinceId, int limit);
}