using System.Data;
using Model.Data.Repository;

namespace Model.Data;

public class UnitOfWorkDapper(IDbConnectionFactory dbConnectionFactory) : IUnitOfWork
{
    public async Task<TResponse> ExecuteAndCommitAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead,
        CancellationToken cancellationToken = default)
    {
        using var dbConnection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken)
            .ConfigureAwait(false);

        using var dbTransaction = dbConnection.BeginTransaction(isolationLevel);
        try
        {
            var repositories = new RepositoryRegistryDapper(dbConnection, dbTransaction);
            var result = await func(repositories).ConfigureAwait(false);
            dbTransaction.Commit();
            return result;
        }
        catch
        {
            dbTransaction.Rollback();
            throw;
        }
    }

    public Task<TResponse> ExecuteWithoutCommitAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead,
        CancellationToken cancellationToken = default)
    {
        return ExecuteInternalAsync(func, cancellationToken);
    }

    private async Task<TResponse> ExecuteInternalAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        CancellationToken cancellationToken)
    {
        using var dbConnection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken)
            .ConfigureAwait(false);

        var repositories = new RepositoryRegistryDapper(dbConnection, transaction: null);
        var result = await func(repositories).ConfigureAwait(false);
        return result;
    }
}