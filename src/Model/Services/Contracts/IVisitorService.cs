using Model.Domain;
using Model.Results;

namespace Model.Services;

public interface IVisitorService
{
    public Task<Visitor?> TryGetAsync(int id);
    
    public Task<Result<Visitor>> CreateAsync(Visitor visitor);
    
    public Task<Result<Visitor>> UpdateAsync(Visitor visitor);
    
    public Task<bool> DeleteAsync(int id);

    public Task<IReadOnlyCollection<Visitor>> GetAll();
}