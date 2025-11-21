namespace Model.Data.Repository;

public class RepositoryRegistryEf(DatabaseContext context) : IRepositoryRegistry
{
    private IVisitorRepository? _visitorRepository;
    public IVisitorRepository VisitorRepository
    {
        get
        {
            return _visitorRepository ??= new VisitorRepositoryEf(context);
        }
    }
}