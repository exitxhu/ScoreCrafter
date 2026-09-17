using System.Globalization;


namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

internal sealed class FormulaParser
{
    private readonly IReadOnlyList<FormulaToken> _tokens;
    private int _position;

    public FormulaParser(string formula)
    {
        _tokens = new FormulaLexer(formula).Tokenize();
    }

    public Expression Parse()
    {
        var expression = ParseExpression();

        Expect(FormulaTokenType.End);

        return expression;
    }

    private Expression ParseExpression()
        => ParseConditional();

    private Expression ParseConditional()
    {
        var condition = ParseLogicalOr();

        if (!Match(FormulaTokenType.Question))
            return condition;

        var whenTrue = ParseExpression();

        Expect(FormulaTokenType.Colon);

        var whenFalse = ParseExpression();

        return new ConditionalExpression(
            condition,
            whenTrue,
            whenFalse);
    }

    private Expression ParseLogicalOr()
    {
        var left = ParseLogicalAnd();

        while (IsKeyword("OR"))
        {
            Next();

            var right = ParseLogicalAnd();

            left = new LogicalGroupOperation(
                left,
                right,
                EnLogicalGroupOperand.Or);
        }

        return left;
    }

    private Expression ParseLogicalAnd()
    {
        var left = ParseComparison();

        while (IsKeyword("AND"))
        {
            Next();

            var right = ParseComparison();

            left = new LogicalGroupOperation(
                left,
                right,
                EnLogicalGroupOperand.And);
        }

        return left;
    }

    private Expression ParseComparison()
    {
        var left = ParseAdditive();

        var operand = Current.Type switch
        {
            FormulaTokenType.Equal =>
                EnLogicalOperand.EQ,

            FormulaTokenType.NotEqual =>
                EnLogicalOperand.NE,

            FormulaTokenType.Greater =>
                EnLogicalOperand.GT,

            FormulaTokenType.GreaterOrEqual =>
                EnLogicalOperand.EG,

            FormulaTokenType.Less =>
                EnLogicalOperand.LT,

            FormulaTokenType.LessOrEqual =>
                EnLogicalOperand.EL,

            _ => (EnLogicalOperand?)null
        };

        if (operand is null)
            return left;

        Next();

        var right = ParseAdditive();

        return new LogicalOperation(
            left,
            right,
            operand.Value);
    }

    private Expression ParseAdditive()
    {
        var left = ParseMultiplicative();

        while (
            Current.Type == FormulaTokenType.Plus ||
            Current.Type == FormulaTokenType.Minus)
        {
            var operand =
                Current.Type == FormulaTokenType.Plus
                    ? EnMathOperand.Add
                    : EnMathOperand.Sub;

            Next();

            var right = ParseMultiplicative();

            left = new Operations(
                left,
                right,
                operand);
        }

        return left;
    }

    private Expression ParseMultiplicative()
    {
        var left = ParseUnary();

        while (
            Current.Type == FormulaTokenType.Multiply ||
            Current.Type == FormulaTokenType.Divide)
        {
            var operand =
                Current.Type == FormulaTokenType.Multiply
                    ? EnMathOperand.Mul
                    : EnMathOperand.Div;

            Next();

            var right = ParseUnary();

            left = new Operations(
                left,
                right,
                operand);
        }

        return left;
    }

    private Expression ParseUnary()
    {
        if (Match(FormulaTokenType.Plus))
        {
            return new UnaryExpression(
                EnUnaryOperand.Plus,
                ParseUnary());
        }

        if (Match(FormulaTokenType.Minus))
        {
            return new UnaryExpression(
                EnUnaryOperand.Minus,
                ParseUnary());
        }

        return ParsePrimary();
    }

    private Expression ParsePrimary()
    {
        if (Match(
                FormulaTokenType.LeftParenthesis))
        {
            var expression = ParseExpression();

            Expect(
                FormulaTokenType.RightParenthesis);

            return expression;
        }

        if (Current.Type == FormulaTokenType.Number)
        {
            var token = Next();

            var value = decimal.Parse(
                token.Value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture);

            return new ConstantExpression(value);
        }

        if (Current.Type == FormulaTokenType.Identifier)
        {
            var identifier = Next();

            if (!Match(
                    FormulaTokenType.LeftParenthesis))
            {
                return new VariableExpression(
                    identifier.Value);
            }

            var arguments = new List<Expression>();

            if (!Match(
                    FormulaTokenType.RightParenthesis))
            {
                do
                {
                    arguments.Add(ParseExpression());
                }
                while (Match(FormulaTokenType.Comma));

                Expect(
                    FormulaTokenType.RightParenthesis);
            }

            return new FunctionExpression(
                identifier.Value,
                arguments);
        }

        throw Error(
            $"Unexpected token '{Current.Value}'.");
    }

    private bool IsKeyword(string keyword)
    {
        return Current.Type == FormulaTokenType.Identifier &&
               string.Equals(
                   Current.Value,
                   keyword,
                   StringComparison.OrdinalIgnoreCase);
    }

    private FormulaToken Next()
    {
        var token = Current;

        if (_position < _tokens.Count - 1)
            _position++;

        return token;
    }

    private bool Match(FormulaTokenType type)
    {
        if (Current.Type != type)
            return false;

        Next();
        return true;
    }

    private void Expect(FormulaTokenType type)
    {
        if (Current.Type != type)
            throw Error(
                $"Expected '{type}' but found '{Current.Value}'.");

        Next();
    }

    private FormulaParseException Error(string message)
        => new(message, Current.Position);

    private FormulaToken Current
        => _tokens[_position];
}
