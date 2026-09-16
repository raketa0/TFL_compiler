namespace Lexemes.UnitTests;

public class BoolLiteralTests
{
    [Fact]
    public void True_produces_bool_literal_token()
    {
        List<Token> actual = LexerHelper.Tokenize("true");

        Assert.Single(actual);
        Assert.Equal(TokenType.BoolLiteral, actual[0].Type);
        Assert.Equal(new TokenValue(true), actual[0].Value);
    }

    [Fact]
    public void False_produces_bool_literal_token()
    {
        List<Token> actual = LexerHelper.Tokenize("false");

        Assert.Single(actual);
        Assert.Equal(TokenType.BoolLiteral, actual[0].Type);
        Assert.Equal(new TokenValue(false), actual[0].Value);
    }

    [Theory]
    [MemberData(nameof(GetBoolLiterals))]
    public void Can_tokenize_bool_literals(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetBoolLiterals()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                "true false",
                [
                    new Token(TokenType.BoolLiteral, new TokenValue(true)),
                    new Token(TokenType.BoolLiteral, new TokenValue(false)),
                ]
            },
            {
                "true false true",
                [
                    new Token(TokenType.BoolLiteral, new TokenValue(true)),
                    new Token(TokenType.BoolLiteral, new TokenValue(false)),
                    new Token(TokenType.BoolLiteral, new TokenValue(true)),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetBoolPrefixIdentifiers))]
    public void Bool_keyword_prefix_is_tokenized_as_identifier(string code, string expectedValue)
    {
        List<Token> tokens = LexerHelper.Tokenize(code);

        Assert.Single(tokens);
        Assert.Equal(TokenType.Identifier, tokens[0].Type);
        Assert.Equal(new TokenValue(expectedValue), tokens[0].Value);
    }

    public static TheoryData<string, string> GetBoolPrefixIdentifiers()
    {
        return new TheoryData<string, string>
        {
            { "trueVal",  "trueVal" },
            { "trueness", "trueness" },
            { "falsehood", "falsehood" },
            { "truer",    "truer" },
        };
    }
}