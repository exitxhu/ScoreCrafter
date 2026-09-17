using ScoreCrafter.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreCrafter.Application.Dtos;


public sealed class FormulaDto
{
    public Guid FormulaId { get; init; }
    public string Definition { get; init; } = string.Empty;
    public int Version { get; init; }

    public static FormulaDto From(Formula formula)
        => new()
        {
            FormulaId = formula.Id,
            Definition = formula.Definition,
            Version = formula.Version
        };
}
