namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

public sealed class FormulaParseException : Exception
{
    public int Position { get; }

    public FormulaParseException(
        string message,
        int position)
        : base($"{message} Position: {position}.")
    {
        Position = position;
    }
}
