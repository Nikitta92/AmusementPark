using System.Data;
using Model.Data.Repository;

namespace Model.Data;

public interface IUnitOfWork
{
    Task<TResponse> ExecuteAndCommitAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead,
        CancellationToken cancellationToken = default);

    Task<TResponse> ExecuteWithoutCommitAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead,
        CancellationToken cancellationToken = default);
}