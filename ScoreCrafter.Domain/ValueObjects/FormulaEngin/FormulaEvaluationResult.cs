namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

public record FormulaEvaluationResult
{
    public FormulaResultType ResultType { get; private set; }

    public bool? BooleanResult { get; private set; }

    public decimal? DecimalResult { get; private set; }

    public FormulaEvaluationResult(decimal? decimalResult = null, bool? booleanResult = null)
    {
        if (decimalResult is not null && booleanResult is not null)
            throw new FormatException("A formula result cannot contain both decimal and boolean values.");
        if (decimalResult is null && booleanResult is null)
            throw new FormatException("A formula result must contain either a decimal or boolean value.");
        DecimalResult = decimalResult;
        BooleanResult = booleanResult;

        ResultType = decimalResult.HasValue
            ? FormulaResultType.Decimal
            : FormulaResultType.Boolean;
    }
    public enum FormulaResultType
    {
        Decimal,
        Boolean
    }
    public decimal AsDecimal()
    {
        if (ResultType != FormulaResultType.Decimal)
            throw new FormulaEvaluationException(
                "Formula result is not decimal.");

        return DecimalResult!.Value;
    }

    public bool AsBoolean()
    {
        if (ResultType != FormulaResultType.Boolean)
            throw new FormulaEvaluationException(
                "Formula result is not boolean.");

        return BooleanResult!.Value;
    }
}
