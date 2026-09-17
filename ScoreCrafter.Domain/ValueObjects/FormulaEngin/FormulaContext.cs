namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

public sealed record FormulaContext(
    decimal PurchaseAmount,
    int PurchaseCount)
{
    public decimal GetValue(string name)
    {
        return name.ToUpperInvariant() switch
        {
            "PURCHASEAMOUNT" => PurchaseAmount,
            "PURCHASECOUNT" => PurchaseCount,
            _ => throw new FormulaEvaluationException(
                $"Unknown variable '{name}'.")
        };
    }
}
