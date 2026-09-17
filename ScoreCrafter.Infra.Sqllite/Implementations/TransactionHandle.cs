using Microsoft.EntityFrameworkCore.Storage;

using ScoreCrafter.Application.Facilatores;

namespace ScoreCrafter.Infra.Sqllite.Implementations;

public sealed class TransactionHandle : IAsyncDisposable, ITransactionHandle
{
    private readonly IDbContextTransaction _transaction;

    public bool OwnsTransaction { get; }

    internal TransactionHandle(
        IDbContextTransaction transaction,
        bool ownsTransaction)
    {
        _transaction = transaction;
        OwnsTransaction = ownsTransaction;
    }

    public async Task CommitAsync(
        CancellationToken cancellationToken = default)
    {
        if (!OwnsTransaction)
            return;

        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(
        CancellationToken cancellationToken = default)
    {
        if (!OwnsTransaction)
            return;

        await _transaction.RollbackAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (OwnsTransaction)
            await _transaction.DisposeAsync();
    }
}