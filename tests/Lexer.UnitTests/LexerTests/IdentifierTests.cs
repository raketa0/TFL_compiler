namespace Lexemes.UnitTests;

public class IdentifierTests
{
    [Theory]
    [MemberData(nameof(GetIdentifiers))]
    public void Can_tokenize_identifiers(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetIdentifiers()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "x",         [new Token(TokenType.Identifier, new TokenValue("x"))] },
            { "foo",       [new Token(TokenType.Identifier, new TokenValue("foo"))] },
            { "_bar",      [new Token(TokenType.Identifier, new TokenValue("_bar"))] },
            { "foo123",    [new Token(TokenType.Identifier, new TokenValue("foo123"))] },
            { "_",         [new Token(TokenType.Identifier, new TokenValue("_"))] },
            { "camelCase", [new Token(TokenType.Identifier, new TokenValue("camelCase"))] },
            {
                "a b",
                [
                    new Token(TokenType.Identifier, new TokenValue("a")),
                    new Token(TokenType.Identifier, new TokenValue("b")),
                ]
            },
        };
    }
}