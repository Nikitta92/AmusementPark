using System.Data;

namespace Model.Data.Repository;

public class RepositoryRegistryDapper(IDbConnection dbConnection, IDbTransaction? transaction) : IRepositoryRegistry
{
    private IVisitorRepository? _visitorRepository;
    public IVisitorRepository VisitorRepository =>
        _visitorRepository ??= new VisitorRepositoryDapper(transaction?.Connection ?? dbConnection);
}