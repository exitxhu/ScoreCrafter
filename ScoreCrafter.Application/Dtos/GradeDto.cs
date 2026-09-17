using ScoreCrafter.Domain.Entities;

namespace ScoreCrafter.Application.Dtos;

public sealed class GradeDto
{
    public int GradeId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Criteria { get; init; } = string.Empty;

    public static GradeDto From(Grade grade)
        => new()
        {
            GradeId = grade.Id,
            Name = grade.Name,
            Description = grade.Description
        };
}
