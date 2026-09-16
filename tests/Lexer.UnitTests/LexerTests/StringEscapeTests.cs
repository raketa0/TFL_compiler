namespace Lexemes.UnitTests;

public class StringEscapeTests
{
    [Theory]
    [MemberData(nameof(GetEscapeSequences))]
    public void Can_decode_escape_sequences(string code, string expectedValue)
    {
        List<Token> actual = LexerHelper.Tokenize(code);

        Assert.Single(actual);
        Assert.Equal(TokenType.StringLiteral, actual[0].Type);
        Assert.Equal(new TokenValue(expectedValue), actual[0].Value);
    }

    public static TheoryData<string, string> GetEscapeSequences()
    {
        return new TheoryData<string, string>
        {
            { "\"\\n\"", "\n" },
            { "\"\\t\"", "\t" },
            { "\"\\\"\"", "\"" },
            { "\"\\\\\"", "\\" },
            { "\"hello\\nworld\"", "hello\nworld" },
            { "\"a\\tb\"", "a\tb" },
            { "\"say \\\"hi\\\"\"", "say \"hi\"" },
            { "\"path\\\\file\"", "path\\file" },
        };
    }

    [Fact]
    public void Returns_error_token_for_unknown_escape_sequence()
    {
        List<Token> actual = LexerHelper.Tokenize("\"\\q\"");

        Assert.Single(actual);
        Assert.Equal(TokenType.Error, actual[0].Type);
    }
}