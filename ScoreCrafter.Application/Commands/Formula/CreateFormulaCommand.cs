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

        FormulaCompiler.Build(command.Definition);

        var formula = new Domain.Entities.Formula
        {
            Id = Guid.CreateVersion7(),
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

