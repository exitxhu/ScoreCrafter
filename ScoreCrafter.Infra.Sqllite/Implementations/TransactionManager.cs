using ScoreCrafter.Infra.Sqllite.Persistenc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ScoreCrafter.Application.Facilatores;

using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreCrafter.Infra.Sqllite.Implementations;


public sealed class TransactionManager : ITransactionManager
{
    private readonly ScoreCrafterDbContext _context;

    public TransactionManager(ScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task<ITransactionHandle> BeginAsync(
        CancellationToken cancellationToken = default)
    {
        var currentTransaction = _context.Database.CurrentTransaction;

        if (currentTransaction is not null)
        {
            return new TransactionHandle(
                currentTransaction,
                ownsTransaction: false);
        }

        var transaction =
            await _context.Database.BeginTransactionAsync(
                cancellationToken);

        return new TransactionHandle(
            transaction,
            ownsTransaction: true);
    }
}
