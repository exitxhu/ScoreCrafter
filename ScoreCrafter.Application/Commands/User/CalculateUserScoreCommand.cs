namespace ScoreCrafter.Application.Commands.User;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Domain.Entities;
using ScoreCrafter.Domain.ValueObjects.FormulaEngin;

using Microsoft.EntityFrameworkCore;

using System;

public sealed record CalculateUserScoreCommand(
    Guid UserId,
    Guid PurchaseId);


public sealed class CalculateUserScoreCommandHandler
{
    private readonly IScoreCrafterDbContext _context;

    public CalculateUserScoreCommandHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        CalculateUserScoreCommand command,
        CancellationToken cancellationToken = default)
    {
        var userExists = await _context.Users
            .AnyAsync(
                x => x.Id == command.UserId,
                cancellationToken);

        if (!userExists)
            throw new InvalidOperationException(
                $"User '{command.UserId}' does not exist.");

        var purchaseExists = await _context.Purchases
            .AnyAsync(
                x =>
                    x.Id == command.PurchaseId &&
                    x.UserId == command.UserId,
                cancellationToken);

        if (!purchaseExists)
            throw new InvalidOperationException(
                $"Purchase '{command.PurchaseId}' does not exist.");

        var purchaseAmount = await _context.Purchases
            .Where(x =>
                x.UserId == command.UserId &&
                x.Id == command.PurchaseId)
            .Select(x => x.Amount)
            .SingleAsync(cancellationToken);

        var purchaseCount = await _context.Purchases
            .CountAsync(
                x => x.UserId == command.UserId,
                cancellationToken);

        var currentGrade = await _context.UserGrades
            .Where(x =>
                x.UserId == command.UserId &&
                x.IsCurrent)
            .Select(x => new
            {
                x.Grade.Id,
                
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (currentGrade is null)
            return;

        var formula = await _context.Formulas
            .Where(x => x.GradeId == currentGrade.Id)
            .OrderByDescending(x => x.Version)
            .FirstOrDefaultAsync(cancellationToken);

        if (formula is null)
            return;

        var compiledFormula =
            FormulaCompiler.Build(formula.Definition);

        var result = compiledFormula.Evaluate(
            new FormulaContext(
                purchaseAmount,
                purchaseCount,
                currentGrade.Id));

        var score = result.AsDecimal();

        var currentScores = await _context.UserScores
            .Where(x =>
                x.UserId == command.UserId &&
                x.IsCurrent)
            .ToListAsync(cancellationToken);

        foreach (var currentScore in currentScores)
        {
            currentScore.IsCurrent = false;
        }

        var userScore = new UserScore
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            Score = score,
            FormulaId = formula.Id,
            FormulaVersion = formula.Version,
            CalculatedAt = DateTime.UtcNow,
            IsCurrent = true
        };

        await _context.UserScores.AddAsync(
            userScore,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
