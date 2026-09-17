using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Domain.Entities;
using ScoreCrafter.Domain.ValueObjects.FormulaEngin;

using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using ScoreCrafter.Application.Dtos;

namespace ScoreCrafter.Application.Commands.Formula;

public sealed record CreateFormulaCommand(
    int GradeId,
    string Definition,
    int Version);


public sealed class CreateFormulaCommandHandler
{
    private readonly IScoreCrafterDbContext _context;

    public CreateFormulaCommandHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task<FormulaDto> Handle(
        CreateFormulaCommand command,
        CancellationToken cancellationToken)
    {
        var gradeExists = await _context.Grades
            .AnyAsync(
                x => x.Id == command.GradeId,
                cancellationToken);

        if (!gradeExists)
            throw new KeyNotFoundException("Grade not found.");

        FormulaCompiler.Build(command.Definition);

        var formula = new Domain.Entities.Formula
        {
            Id = Guid.CreateVersion7(),
            GradeId = command.GradeId,
            Definition = command.Definition,
            Version = command.Version
        };

        await _context.Formulas.AddAsync(
            formula,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return FormulaDto.From(formula);
    }
}

