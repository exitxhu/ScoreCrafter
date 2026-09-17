namespace ScoreCrafter.Application.Dtos;

public sealed class TestGradeDto
{
    public bool IsValid { get; init; }
    public bool Result { get; init; }
    public string? Error { get; init; }
}