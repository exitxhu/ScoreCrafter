using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreCrafter.Application.Facilatores;

public interface ITransactionManager
{
    Task<ITransactionHandle> BeginAsync(CancellationToken cancellationToken = default);
}
public interface ITransactionHandle
{
    bool OwnsTransaction { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
    ValueTask DisposeAsync();
    Task RollbackAsync(CancellationToken cancellationToken = default);
}