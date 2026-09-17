namespace ScoreCrafter.Application.Commands.Formula;

using global::ScoreCrafter.Application.Abstraction.Data;
using global::ScoreCrafter.Domain.ValueObjects.FormulaEngin;

using Microsoft.EntityFrameworkCore;

using ScoreCrafter.Application.Dtos;

public sealed class TestFormulaCommandHandler
{
    private readonly IScoreCrafterDbContext _context;

    public TestFormulaCommandHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task<TestFormulaDto> Handle(
        TestFormulaCommand command,
        CancellationToken cancellationToken)
    {
        var formulaQuery = _context.Formulas
            .Where(x => x.Id == command.FormulaId);

        if (command.Version.HasValue)
        {
            formulaQuery = formulaQuery.Where(
                x => x.Version == command.Version.Value);
        }

        var formula = await formulaQuery
            .FirstOrDefaultAsync(cancellationToken);

        if (formula is null)
            throw new KeyNotFoundException("Formula not found.");

        try
        {
            var compiler = FormulaCompiler.Build(
                formula.Definition);

            var result = compiler.Evaluate(
                new FormulaContext(
                    command.PurchaseAmount,
                    command.PurchaseCount,
                    command.UserGrade));

            return new TestFormulaDto
            {
                IsValid = true,
                DecimalResult = result.DecimalResult,
                BooleanResult = result.BooleanResult
            };
        }
        catch (Exception ex)
        {
            return new TestFormulaDto
            {
                IsValid = false,
                Error = ex.Message
            };
        }
    }
}
public sealed record TestFormulaCommand(
    Guid FormulaId,
    decimal PurchaseAmount,
    int PurchaseCount,
    int UserGrade,
    int? Version);

