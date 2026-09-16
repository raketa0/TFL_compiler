namespace Lexemes.UnitTests;

public class ErrorTests
{
    [Theory]
    [MemberData(nameof(GetUnknownCharacters))]
    public void Returns_error_token_for_unknown_characters(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetUnknownCharacters()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "@", [new Token(TokenType.Error, new TokenValue("@"))] },
            { "$", [new Token(TokenType.Error, new TokenValue("$"))] },
            { "?", [new Token(TokenType.Error, new TokenValue("?"))] },

            // ошибка среди валидных токенов
            {
                "@ x",
                [
                    new Token(TokenType.Error, new TokenValue("@")),
                    new Token(TokenType.Identifier, new TokenValue("x")),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetIncompleteOperators))]
    public void Returns_error_token_for_incomplete_operators(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetIncompleteOperators()
    {
        return new TheoryData<string, List<Token>>()
        {
            // одиночный & не является валидным оператором
            { "&", [new Token(TokenType.Error, new TokenValue("&"))] },

            // одиночный | не является валидным оператором
            { "|", [new Token(TokenType.Error, new TokenValue("|"))] },

            // одиночный & среди валидных токенов
            {
                "x & y",
                [
                    new Token(TokenType.Identifier, new TokenValue("x")),
                    new Token(TokenType.Error, new TokenValue("&")),
                    new Token(TokenType.Identifier, new TokenValue("y")),
                ]
            },
        };
    }

    [Fact]
    public void Returns_error_token_for_unterminated_string()
    {
        List<Token> actual = LexerHelper.Tokenize("\"hello");

        Assert.Single(actual);
        Assert.Equal(TokenType.Error, actual[0].Type);
    }
}