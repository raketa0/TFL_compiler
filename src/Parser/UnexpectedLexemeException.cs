using Lexemes;

namespace Parsing;

#pragma warning disable RCS1194
public class UnexpectedLexemeException : Exception
{
    public UnexpectedLexemeException(Token actual, TokenType expected)
    : base($"Unexpected lexeme {actual} where expected {expected}")
    {
    }

    public UnexpectedLexemeException(Token actual, IEnumerable<TokenType> expected)
        : base($"Unexpected lexeme {actual} where expected one of {string.Join(", ", expected)}")
    {
    }
}
#pragma warning restore RCS1194