namespace Lexemes.UnitTests;

public class KeywordsTests
{
    [Theory]
    [MemberData(nameof(GetKeywords))]
    public void Can_tokenize_keywords(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetKeywords()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "main",  [new Token(TokenType.Main)] },
            { "var",   [new Token(TokenType.Var)] },
            { "const", [new Token(TokenType.Const)] },
            { "int",   [new Token(TokenType.IntegerType)] },
            { "read",  [new Token(TokenType.Read)] },
            { "print", [new Token(TokenType.Print)] },
            {
                "var const int",
                [
                    new Token(TokenType.Var),
                    new Token(TokenType.Const),
                    new Token(TokenType.IntegerType),
                ]
            },
            { "func",   [new Token(TokenType.Func)] },
            { "return", [new Token(TokenType.Return)] },
            { "void",   [new Token(TokenType.Void)] },

            // слова, начинающиеся как ключевые — должны быть Identifier
            { "mainX",     [new Token(TokenType.Identifier, new TokenValue("mainX"))] },
            { "variable",  [new Token(TokenType.Identifier, new TokenValue("variable"))] },
            { "consts",    [new Token(TokenType.Identifier, new TokenValue("consts"))] },
            { "integer",   [new Token(TokenType.Identifier, new TokenValue("integer"))] },
            { "reading",   [new Token(TokenType.Identifier, new TokenValue("reading"))] },
            { "funcX",     [new Token(TokenType.Identifier, new TokenValue("funcX"))] },
            { "functions", [new Token(TokenType.Identifier, new TokenValue("functions"))] },
            { "returning", [new Token(TokenType.Identifier, new TokenValue("returning"))] },
            { "returns2",  [new Token(TokenType.Identifier, new TokenValue("returns2"))] },
            { "voidX",     [new Token(TokenType.Identifier, new TokenValue("voidX"))] },
        };
    }
}