namespace ScoreCrafter.Domain.ValueObjects.FormulaEngin;

internal enum FormulaTokenType
{
    Number,
    Identifier,

    Plus,
    Minus,
    Multiply,
    Divide,

    Equal,
    NotEqual,
    Greater,
    GreaterOrEqual,
    Less,
    LessOrEqual,

    LeftParenthesis,
    RightParenthesis,
    Comma,

    Question,
    Colon,

    End
}
internal readonly record struct FormulaToken(
    FormulaTokenType Type,
    string Value,
    int Position);