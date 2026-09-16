namespace Lexemes.UnitTests;

public class SeparatorTests
{
    [Theory]
    [MemberData(nameof(GetSeparators))]
    public void Can_tokenize_separators(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetSeparators()
    {
        return new TheoryData<string, List<Token>>()
        {
            { ",", [new Token(TokenType.Comma)] },
            { ";", [new Token(TokenType.Semicolon)] },
            { ":", [new Token(TokenType.Colon)] },
            { "(", [new Token(TokenType.OpenParenthesis)] },
            { ")", [new Token(TokenType.CloseParenthesis)] },
            { "{", [new Token(TokenType.OpenBrace)] },
            { "}", [new Token(TokenType.CloseBrace)] },
            {
                "( )",
                [
                    new Token(TokenType.OpenParenthesis),
                    new Token(TokenType.CloseParenthesis),
                ]
            },
            {
                "{ }",
                [
                    new Token(TokenType.OpenBrace),
                    new Token(TokenType.CloseBrace),
                ]
            },
        };
    }
}