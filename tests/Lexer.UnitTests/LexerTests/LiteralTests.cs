namespace Lexemes.UnitTests;

public class LiteralTests
{
    [Theory]
    [MemberData(nameof(GetIntLiterals))]
    public void Can_tokenize_int_literals(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetIntLiterals()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "0",    [new Token(TokenType.IntLiteral, new TokenValue(0))] },
            { "1",    [new Token(TokenType.IntLiteral, new TokenValue(1))] },
            { "42",   [new Token(TokenType.IntLiteral, new TokenValue(42))] },
            { "1000", [new Token(TokenType.IntLiteral, new TokenValue(1000))] },
            {
                "1 2 3",
                [
                    new Token(TokenType.IntLiteral, new TokenValue(1)),
                    new Token(TokenType.IntLiteral, new TokenValue(2)),
                    new Token(TokenType.IntLiteral, new TokenValue(3)),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetFloatLiterals))]
    public void Can_tokenize_float_literals(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetFloatLiterals()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "0.0",  [new Token(TokenType.FloatLiteral, new TokenValue(0.0))] },
            { "1.5",  [new Token(TokenType.FloatLiteral, new TokenValue(1.5))] },
            { "3.14", [new Token(TokenType.FloatLiteral, new TokenValue(3.14))] },
            {
                "0.0 1.5 3.14",
                [
                    new Token(TokenType.FloatLiteral, new TokenValue(0.0)),
                    new Token(TokenType.FloatLiteral, new TokenValue(1.5)),
                    new Token(TokenType.FloatLiteral, new TokenValue(3.14)),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetStringLiterals))]
    public void Can_tokenize_string_literals(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetStringLiterals()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "\"\"",           [new Token(TokenType.StringLiteral, new TokenValue(""))] },
            { "\"hello\"",      [new Token(TokenType.StringLiteral, new TokenValue("hello"))] },
            { "\"hello world\"", [new Token(TokenType.StringLiteral, new TokenValue("hello world"))] },
        };
    }
}