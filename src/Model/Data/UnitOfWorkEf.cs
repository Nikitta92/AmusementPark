using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Data.Repository;
using Model.Domain;

namespace Model.Data;

public sealed class UnitOfWorkEf(
    IDbContextFactory<DatabaseContext> dbContextFactory,
    ILogger<UnitOfWorkEf> logger) : IUnitOfWork
{
    public async Task ExecuteAndCommitAsync<TResponse>(
        Func<IRepositoryRegistry, Task> func,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead,
        CancellationToken cancellationToken = default)
    {
        await ExecuteAndCommitAsync(async x =>
        {
            await func.Invoke(x);
            return (object?)null;
        }, isolationLevel, cancellationToken);
    }

    public async Task<TResponse> ExecuteAndCommitAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead,
        CancellationToken cancellationToken = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        var result = await func(new RepositoryRegistryEf(context));
        await context.SaveChangesAsync(cancellationToken);
        await context.Database.CommitTransactionAsync(cancellationToken);
        return result;
    }

    public async Task<TResponse> ExecuteWithoutCommitAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead,
        CancellationToken cancellationToken = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await func(new RepositoryRegistryEf(context));
        return result;
    }

    public async Task<TResponse> ExecuteAndCommitWithRetryAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        int maxRetryCount = 3,
        IsolationLevel isolationLevel = IsolationLevel.RepeatableRead,
        CancellationToken cancellationToken = default)
    {
        var retryCount = 0;
        while (retryCount < maxRetryCount)
        {
            try
            {
                return await ExecuteAndCommitAsync(func, isolationLevel, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error executing and commit transaction");
                retryCount++;
            }
        }

        throw new InvalidOperationException("Failed to execute and commit transaction with retries");
    }
}