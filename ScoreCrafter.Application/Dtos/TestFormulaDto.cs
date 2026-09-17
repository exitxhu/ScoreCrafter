namespace ScoreCrafter.Application.Dtos;

public sealed class TestFormulaDto
{
    public bool IsValid { get; init; }
    public decimal? DecimalResult { get; init; }
    public bool? BooleanResult { get; init; }
    public string? Error { get; init; }
}