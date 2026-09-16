namespace Lexemes.UnitTests;

public class LexerTests
{
    [Theory]
    [MemberData(nameof(GetTokenizeIdentifiersAndKeywordsData))]
    [MemberData(nameof(GetTokenizeLiteralsData))]
    [MemberData(nameof(GetSkipWhitespacesAndCommentsData))]
    [MemberData(nameof(GetTokenizePunctuationData))]
    public void Can_tokenize_lexemes(string code, List<Token> expected)
    {
        List<Token> actual = Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetTokenizeIdentifiersAndKeywordsData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                "alice Bob C00L D_2 x", [
                    new Token(TokenType.Identifier, new TokenValue("alice")),
                    new Token(TokenType.Identifier, new TokenValue("Bob")),
                    new Token(TokenType.Identifier, new TokenValue("C00L")),
                    new Token(TokenType.Identifier, new TokenValue("D_2")),
                    new Token(TokenType.Identifier, new TokenValue("x"))
                ]
            },
            {
                "const X: int = 102;", [
                    new Token(TokenType.Const),
                    new Token(TokenType.Identifier, new TokenValue("X")),
                    new Token(TokenType.Colon),
                    new Token(TokenType.IntegerType),
                    new Token(TokenType.Assign),
                    new Token(TokenType.IntLiteral, new TokenValue(102)),
                    new Token(TokenType.Semicolon),
                ]
            },
        };
    }

    public static TheoryData<string, List<Token>> GetTokenizeLiteralsData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                "0 1234 56789 0017", [
                    new Token(TokenType.IntLiteral, new TokenValue(0)),
                    new Token(TokenType.IntLiteral, new TokenValue(1234)),
                    new Token(TokenType.IntLiteral, new TokenValue(56789)),
                    new Token(TokenType.IntLiteral, new TokenValue(17)),
                ]
            },
        };
    }

    public static TheoryData<string, List<Token>> GetSkipWhitespacesAndCommentsData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                // Пропуск пробельных символов.
                "x \t\r\n\fy", [
                    new Token(TokenType.Identifier, new TokenValue("x")),
                    new Token(TokenType.Identifier, new TokenValue("y")),
                ]
            },
            {
                // Пропуск комментариев.
                "# comments # \na / # should be # \nb * c # ignored #", [
                    new Token(TokenType.Identifier, new TokenValue("a")),
                    new Token(TokenType.Divide),
                    new Token(TokenType.Identifier, new TokenValue("b")),
                    new Token(TokenType.Multiply),
                    new Token(TokenType.Identifier, new TokenValue("c")),
                ]
            },
            {
                // Пропуск вложенных комментариев.
                "a / b # nested # comments # are allowed # \n* c", [
                    new Token(TokenType.Identifier, new TokenValue("a")),
                    new Token(TokenType.Divide),
                    new Token(TokenType.Identifier, new TokenValue("b")),
                    new Token(TokenType.Multiply),
                    new Token(TokenType.Identifier, new TokenValue("c")),
                ]
            },
        };
    }

    public static TheoryData<string, List<Token>> GetTokenizePunctuationData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                "x + y / (10 - z * 2)", [
                    new Token(TokenType.Identifier, new TokenValue("x")),
                    new Token(TokenType.Plus),
                    new Token(TokenType.Identifier, new TokenValue("y")),
                    new Token(TokenType.Divide),
                    new Token(TokenType.OpenParenthesis),
                    new Token(TokenType.IntLiteral, new TokenValue(10)),
                    new Token(TokenType.Minus),
                    new Token(TokenType.Identifier, new TokenValue("z")),
                    new Token(TokenType.Multiply),
                    new Token(TokenType.IntLiteral, new TokenValue(2)),
                    new Token(TokenType.CloseParenthesis),
                ]
            },
        };
    }

    private static List<Token> Tokenize(string code)
    {
        List<Token> results = [];
        Lexer lexer = new(code);
        for (Token t = lexer.ParseToken(); t.Type != TokenType.EndOfFile; t = lexer.ParseToken())
        {
            results.Add(t);
        }

        return results;
    }
}