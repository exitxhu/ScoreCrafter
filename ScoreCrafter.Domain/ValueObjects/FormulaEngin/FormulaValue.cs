using System.Globalization;


namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

internal readonly record struct FormulaValue
{
    private readonly decimal _decimalValue;
    private readonly bool _booleanValue;

    public FormulaValueType Type { get; }

    private FormulaValue(
        FormulaValueType type,
        decimal decimalValue,
        bool booleanValue)
    {
        Type = type;
        _decimalValue = decimalValue;
        _booleanValue = booleanValue;
    }

    public static FormulaValue Decimal(decimal value)
        => new(FormulaValueType.Decimal, value, false);

    public static FormulaValue Boolean(bool value)
        => new(FormulaValueType.Boolean, 0, value);

    public decimal AsDecimal()
    {
        if (Type != FormulaValueType.Decimal)
            throw new FormulaEvaluationException(
                "Expected a numeric expression.");

        return _decimalValue;
    }

    public bool AsBoolean()
    {
        if (Type != FormulaValueType.Boolean)
            throw new FormulaEvaluationException(
                "Expected a logical expression.");

        return _booleanValue;
    }

    public override string ToString()
        => Type == FormulaValueType.Decimal
            ? _decimalValue.ToString(CultureInfo.InvariantCulture)
            : _booleanValue.ToString();
}
