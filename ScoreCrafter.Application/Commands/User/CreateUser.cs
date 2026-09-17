namespace ScoreCrafter.Application.Commands.User;
using Microsoft.EntityFrameworkCore;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Application.Facilatores;
using ScoreCrafter.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

public sealed record CreateUserCommand(Guid UserId);
public sealed class CreateUserCommandHandler
{
    private readonly IScoreCrafterDbContext _context;
    private readonly ITransactionManager _transactionManager;

    public CreateUserCommandHandler(
        IScoreCrafterDbContext context,
        ITransactionManager transactionManager)
    {
        _context = context;
        _transactionManager = transactionManager;
    }

    public async Task Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _transactionManager.BeginAsync(cancellationToken);

        try
        {
            var exists = await _context.Users
                .AnyAsync(
                    x => x.Id == command.UserId,
                    cancellationToken);

            if (exists)
                return;

            var user = new User
            {
                Id = command.UserId
            };

            await _context.Users.AddAsync(
                user,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
