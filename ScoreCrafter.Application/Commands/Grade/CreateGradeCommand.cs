using Microsoft.EntityFrameworkCore;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Application.Dtos;
using ScoreCrafter.Domain.ValueObjects.FormulaEngin;

using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreCrafter.Application.Commands.Grade;

public sealed record CreateGradeCommand(
    string Name,
    string Description);

public sealed class CreateGradeCommandHandler
{
    private readonly IScoreCrafterDbContext _context;

    public CreateGradeCommandHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task<GradeDto> Handle(
        CreateGradeCommand command,
        CancellationToken cancellationToken)
    {
        var id = 0;
        if (await _context.Grades.AnyAsync())
            id = await _context.Grades.MaxAsync(a => a.Id);
        var grade = new Domain.Entities.Grade
        {
            Id = id + 1,
            Name = command.Name,
            Description = command.Description
        };

        await _context.Grades.AddAsync(
            grade,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return GradeDto.From(grade);
    }
}
