using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Domain.ValueObjects.FormulaEngin;

using Microsoft.EntityFrameworkCore;
using ScoreCrafter.Application.Dtos;

namespace ScoreCrafter.Application.Commands.Formula;

public sealed record UpdateFormulaCommand(
    Guid FormulaId,
    string Definition);


public sealed class UpdateFormulaCommandHandler
{
    private readonly IScoreCrafterDbContext _context;

    public UpdateFormulaCommandHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task<FormulaDto> Handle(
        UpdateFormulaCommand command,
        CancellationToken cancellationToken)
    {
        var current = await _context.Formulas
            .FirstOrDefaultAsync(
                x => x.Id == command.FormulaId,
                cancellationToken);

        if (current is null)
            throw new KeyNotFoundException("Formula not found.");

        FormulaCompiler.Build(command.Definition);

        var nextVersion = await _context.Formulas
            .Where(x => x.GradeId == current.GradeId)
            .MaxAsync(
                x => (int?)x.Version,
                cancellationToken) ?? 0;

        var formula = new Domain.Entities.Formula
        {
            Id = Guid.CreateVersion7(),
            GradeId = current.GradeId,
            Definition = command.Definition,
            Version = nextVersion + 1
        };

        await _context.Formulas.AddAsync(
            formula,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return FormulaDto.From(formula);
    }
}
