namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

public sealed class FormulaEvaluationException : Exception
{
    public FormulaEvaluationException(string message)
        : base(message)
    {
    }
}