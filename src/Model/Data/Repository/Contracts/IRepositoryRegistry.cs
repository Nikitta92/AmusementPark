namespace Model.Data.Repository;

public interface IRepositoryRegistry
{
    IVisitorRepository VisitorRepository { get; }
}