namespace ScoreCrafter.Application.Commands.Purchase;

using Microsoft.EntityFrameworkCore;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Application.Commands.User;
using ScoreCrafter.Application.Facilatores;
using ScoreCrafter.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Text;

public sealed record RegisterPurchaseCommand(
    Guid PurchaseId,
    Guid UserId,
    decimal Amount,
    DateTime PurchaseDate,
    Dictionary<string, string>? Metadata);


public sealed class RegisterPurchaseCommandHandler
{
    private readonly IScoreCrafterDbContext _context;
    private readonly CreateUserCommandHandler _createUserHandler;
    private readonly ITransactionManager _transactionManager;

    public RegisterPurchaseCommandHandler(
        IScoreCrafterDbContext context,
        CreateUserCommandHandler createUserHandler,
        ITransactionManager transactionManager)
    {
        _context = context;
        _createUserHandler = createUserHandler;
        _transactionManager = transactionManager;
    }

    public async Task Handle(
        RegisterPurchaseCommand command,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _transactionManager.BeginAsync(cancellationToken);

        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Id == command.UserId,
                    cancellationToken);

            if (user is null)
            {
                user = new User
                {
                    Id = command.UserId
                };

                await _context.Users.AddAsync(
                    user,
                    cancellationToken);
            }

            var purchase = new Purchase
            {
                Id = command.PurchaseId,
                UserId = command.UserId,
                Amount = command.Amount,
                PurchaseDate = command.PurchaseDate,
                Metadata = command.Metadata ?? new()
            };

            await _context.Purchases.AddAsync(
                purchase,
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