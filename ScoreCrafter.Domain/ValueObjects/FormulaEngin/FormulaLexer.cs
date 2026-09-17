using System.Globalization;


namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

internal sealed class FormulaLexer
{
    private readonly string _input;
    private int _position;

    public FormulaLexer(string input)
    {
        _input = input;
    }

    public IReadOnlyList<FormulaToken> Tokenize()
    {
        var tokens = new List<FormulaToken>();

        while (_position < _input.Length)
        {
            var current = _input[_position];

            if (char.IsWhiteSpace(current))
            {
                _position++;
                continue;
            }

            if (char.IsDigit(current) || current == '.')
            {
                tokens.Add(ReadNumber());
                continue;
            }

            if (char.IsLetter(current) || current == '_')
            {
                tokens.Add(ReadIdentifier());
                continue;
            }

            tokens.Add(ReadOperator());
        }

        tokens.Add(
            new FormulaToken(
                FormulaTokenType.End,
                string.Empty,
                _position));

        return tokens;
    }

    private FormulaToken ReadNumber()
    {
        var start = _position;
        var hasDecimalPoint = false;

        while (_position < _input.Length)
        {
            var current = _input[_position];

            if (char.IsDigit(current))
            {
                _position++;
                continue;
            }

            if (current == '.' && !hasDecimalPoint)
            {
                hasDecimalPoint = true;
                _position++;
                continue;
            }

            break;
        }

        var value = _input[start.._position];

        if (!decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out _))
        {
            throw new FormulaParseException(
                $"Invalid number '{value}'.",
                start);
        }

        return new FormulaToken(
            FormulaTokenType.Number,
            value,
            start);
    }

    private FormulaToken ReadIdentifier()
    {
        var start = _position;

        while (_position < _input.Length)
        {
            var current = _input[_position];

            if (!char.IsLetterOrDigit(current) && current != '_')
                break;

            _position++;
        }

        return new FormulaToken(
            FormulaTokenType.Identifier,
            _input[start.._position],
            start);
    }

    private FormulaToken ReadOperator()
    {
        var start = _position;
        var current = _input[_position];

        _position++;

        return current switch
        {
            '+' => Token(FormulaTokenType.Plus, "+", start),
            '-' => Token(FormulaTokenType.Minus, "-", start),
            '*' => Token(FormulaTokenType.Multiply, "*", start),
            '/' => Token(FormulaTokenType.Divide, "/", start),

            '(' => Token(
                FormulaTokenType.LeftParenthesis,
                "(",
                start),

            ')' => Token(
                FormulaTokenType.RightParenthesis,
                ")",
                start),

            ',' => Token(
                FormulaTokenType.Comma,
                ",",
                start),

            '?' => Token(
                FormulaTokenType.Question,
                "?",
                start),

            ':' => Token(
                FormulaTokenType.Colon,
                ":",
                start),

            '=' when Match('=') =>
                Token(FormulaTokenType.Equal, "==", start),

            '!' when Match('=') =>
                Token(FormulaTokenType.NotEqual, "!=", start),

            '>' when Match('=') =>
                Token(
                    FormulaTokenType.GreaterOrEqual,
                    ">=",
                    start),

            '>' => Token(
                FormulaTokenType.Greater,
                ">",
                start),

            '<' when Match('=') =>
                Token(
                    FormulaTokenType.LessOrEqual,
                    "<=",
                    start),

            '<' => Token(
                FormulaTokenType.Less,
                "<",
                start),

            _ => throw new FormulaParseException(
                $"Unexpected character '{current}'.",
                start)
        };
    }

    private bool Match(char expected)
    {
        if (_position >= _input.Length ||
            _input[_position] != expected)
            return false;

        _position++;
        return true;
    }

    private static FormulaToken Token(
        FormulaTokenType type,
        string value,
        int position)
        => new(type, value, position);
}
