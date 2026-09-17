using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Application.Dtos;
using ScoreCrafter.Domain.ValueObjects.FormulaEngin;
using Microsoft.EntityFrameworkCore;

namespace ScoreCrafter.Application.Commands.Grade;

public sealed record UpdateGradeCommand(
    int GradeId,
    string Description);



public sealed class UpdateGradeCommandHandler
{
    private readonly IScoreCrafterDbContext _context;

    public UpdateGradeCommandHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task<GradeDto> Handle(
        UpdateGradeCommand command,
        CancellationToken cancellationToken)
    {
        var grade = await _context.Grades
            .FirstOrDefaultAsync(
                x => x.Id == command.GradeId,
                cancellationToken);

        if (grade is null)
            throw new KeyNotFoundException("Grade not found.");

        grade.Description = command.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return GradeDto.From(grade);
    }
}
