namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

internal sealed record Operations(
    Expression Left,
    Expression Right,
    EnMathOperand Operand)
    : Expression
{
    public override FormulaValue Evaluate(FormulaContext context)
    {
        var left = Left.Evaluate(context).AsDecimal();
        var right = Right.Evaluate(context).AsDecimal();

        var result = Operand switch
        {
            EnMathOperand.Add => left + right,
            EnMathOperand.Sub => left - right,
            EnMathOperand.Mul => left * right,

            EnMathOperand.Div when right == 0 =>
                throw new FormulaEvaluationException(
                    "Division by zero is not allowed."),

            EnMathOperand.Div => left / right,

            _ => throw new FormulaEvaluationException(
                $"Unsupported math operand '{Operand}'.")
        };

        return FormulaValue.Decimal(result);
    }
}

internal sealed record LogicalOperation(
    Expression Left,
    Expression Right,
    EnLogicalOperand Operand)
    : Expression
{
    public override FormulaValue Evaluate(FormulaContext context)
    {
        var left = Left.Evaluate(context).AsDecimal();
        var right = Right.Evaluate(context).AsDecimal();

        var result = Operand switch
        {
            EnLogicalOperand.EQ => left == right,
            EnLogicalOperand.NE => left != right,
            EnLogicalOperand.GT => left > right,
            EnLogicalOperand.EG => left >= right,
            EnLogicalOperand.LT => left < right,
            EnLogicalOperand.EL => left <= right,

            _ => throw new FormulaEvaluationException(
                $"Unsupported logical operand '{Operand}'.")
        };

        return FormulaValue.Boolean(result);
    }
}

internal sealed record LogicalGroupOperation(
    Expression Left,
    Expression Right,
    EnLogicalGroupOperand Operand)
    : Expression
{
    public override FormulaValue Evaluate(FormulaContext context)
    {
        var left = Left.Evaluate(context).AsBoolean();

        return Operand switch
        {
            EnLogicalGroupOperand.And =>
                FormulaValue.Boolean(
                    left && Right.Evaluate(context).AsBoolean()),

            EnLogicalGroupOperand.Or =>
                FormulaValue.Boolean(
                    left || Right.Evaluate(context).AsBoolean()),

            _ => throw new FormulaEvaluationException(
                $"Unsupported logical group operand '{Operand}'.")
        };
    }
}


internal enum EnMathOperand
{
    Add,
    Sub,
    Mul,
    Div
}

internal enum EnLogicalOperand
{
    EQ,
    NE,
    GT,
    EG,
    LT,
    EL
}

internal enum EnLogicalGroupOperand
{
    And,
    Or
}

internal enum EnUnaryOperand
{
    Plus,
    Minus
}
