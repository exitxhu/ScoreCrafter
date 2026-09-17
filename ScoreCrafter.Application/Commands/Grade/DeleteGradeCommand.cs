using ScoreCrafter.Application.Abstraction.Data;
using Microsoft.EntityFrameworkCore;

namespace ScoreCrafter.Application.Commands.Grade;

public sealed record DeleteGradeCommand(int GradeId);

public sealed class DeleteGradeCommandHandler
{
    private readonly IScoreCrafterDbContext _context;

    public DeleteGradeCommandHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteGradeCommand command,
        CancellationToken cancellationToken)
    {
        var grade = await _context.Grades
            .FirstOrDefaultAsync(
                x => x.Id == command.GradeId,
                cancellationToken);

        if (grade is null)
            throw new KeyNotFoundException("Grade not found.");

        _context.Grades.Remove(grade);

        await _context.SaveChangesAsync(cancellationToken);
    }
}