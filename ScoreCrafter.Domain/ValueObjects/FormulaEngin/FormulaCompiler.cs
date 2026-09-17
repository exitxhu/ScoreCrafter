namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

public sealed record FormulaCompiler
{
    private readonly Expression _expression;

    private FormulaCompiler(Expression expression)
    {
        _expression = expression;
    }

    public static FormulaCompiler Build(string formula)
    {
        if (string.IsNullOrWhiteSpace(formula))
            throw new ArgumentException("Formula cannot be empty.", nameof(formula));

        var parser = new FormulaParser(formula);
        var expression = parser.Parse();

        return new FormulaCompiler(expression);
    }

    public FormulaEvaluationResult Evaluate(FormulaContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var result = _expression.Evaluate(context);

        return result.Type switch
        {
            FormulaValueType.Decimal => new(decimalResult: result.AsDecimal()),
            FormulaValueType.Boolean => new(booleanResult: result.AsBoolean()),
            _ => throw new FormulaEvaluationException($"Unsupported formula result type '{result.Type}'.")

        };
    }

    public override string ToString()
        => _expression.ToString() ?? string.Empty;
}
