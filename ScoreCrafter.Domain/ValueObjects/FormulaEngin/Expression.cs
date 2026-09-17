namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

internal abstract record Expression
{
    public abstract FormulaValue Evaluate(FormulaContext context);
}

internal sealed record ConstantExpression(decimal Value)
    : Expression
{
    public override FormulaValue Evaluate(FormulaContext context)
        => FormulaValue.Decimal(Value);
}

internal sealed record VariableExpression(string Name)
    : Expression
{
    public override FormulaValue Evaluate(FormulaContext context)
        => FormulaValue.Decimal(context.GetValue(Name));
}

internal sealed record UnaryExpression(
    EnUnaryOperand Operand,
    Expression Expression)
    : Expression
{
    public override FormulaValue Evaluate(FormulaContext context)
    {
        var value = Expression.Evaluate(context).AsDecimal();

        return Operand switch
        {
            EnUnaryOperand.Plus => FormulaValue.Decimal(value),
            EnUnaryOperand.Minus => FormulaValue.Decimal(-value),
            _ => throw new FormulaEvaluationException(
                $"Unsupported unary operand '{Operand}'.")
        };
    }
}

internal sealed record ConditionalExpression(
    Expression Condition,
    Expression WhenTrue,
    Expression WhenFalse)
    : Expression
{
    public override FormulaValue Evaluate(FormulaContext context)
    {
        var condition = Condition
            .Evaluate(context)
            .AsBoolean();

        return condition
            ? WhenTrue.Evaluate(context)
            : WhenFalse.Evaluate(context);
    }
}

internal sealed record FunctionExpression(
    string Name,
    IReadOnlyList<Expression> Arguments)
    : Expression
{
    public override FormulaValue Evaluate(FormulaContext context)
    {
        return Name.ToUpperInvariant() switch
        {
            "IF" => EvaluateIf(context),
            "FLOOR" => EvaluateFloor(context),
            "CEILING" => EvaluateCeiling(context),
            "MIN" => EvaluateMin(context),
            "MAX" => EvaluateMax(context),

            _ => throw new FormulaEvaluationException(
                $"Unknown function '{Name}'.")
        };
    }

    private FormulaValue EvaluateIf(FormulaContext context)
    {
        EnsureArgumentCount(3);

        var condition = Arguments[0]
            .Evaluate(context)
            .AsBoolean();

        return condition
            ? Arguments[1].Evaluate(context)
            : Arguments[2].Evaluate(context);
    }

    private FormulaValue EvaluateFloor(FormulaContext context)
    {
        EnsureArgumentCount(1);

        var value = Arguments[0]
            .Evaluate(context)
            .AsDecimal();

        return FormulaValue.Decimal(Math.Floor(value));
    }

    private FormulaValue EvaluateCeiling(FormulaContext context)
    {
        EnsureArgumentCount(1);

        var value = Arguments[0]
            .Evaluate(context)
            .AsDecimal();

        return FormulaValue.Decimal(Math.Ceiling(value));
    }

    private FormulaValue EvaluateMin(FormulaContext context)
    {
        EnsureArgumentCountAtLeast(1);

        var values = Arguments
            .Select(x => x.Evaluate(context).AsDecimal());

        return FormulaValue.Decimal(values.Min());
    }

    private FormulaValue EvaluateMax(FormulaContext context)
    {
        EnsureArgumentCountAtLeast(1);

        var values = Arguments
            .Select(x => x.Evaluate(context).AsDecimal());

        return FormulaValue.Decimal(values.Max());
    }

    private void EnsureArgumentCount(int expected)
    {
        if (Arguments.Count != expected)
            throw new FormulaEvaluationException(
                $"Function '{Name}' expects {expected} arguments.");
    }

    private void EnsureArgumentCountAtLeast(int minimum)
    {
        if (Arguments.Count < minimum)
            throw new FormulaEvaluationException(
                $"Function '{Name}' expects at least {minimum} arguments.");
    }
}