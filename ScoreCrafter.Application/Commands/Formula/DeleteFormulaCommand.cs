namespace ScoreCrafter.Application.Commands.Formula;
using Microsoft.EntityFrameworkCore;

using global::ScoreCrafter.Application.Abstraction.Data;

public sealed class DeleteFormulaCommandHandler
{
    private readonly IScoreCrafterDbContext _context;

    public DeleteFormulaCommandHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteFormulaCommand command,
        CancellationToken cancellationToken)
    {
        var formula = await _context.Formulas
            .FirstOrDefaultAsync(
                x => x.Id== command.FormulaId,
                cancellationToken);

        if (formula is null)
            throw new KeyNotFoundException("Formula not found.");

        _context.Formulas.Remove(formula);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
public sealed record DeleteFormulaCommand(Guid FormulaId);
