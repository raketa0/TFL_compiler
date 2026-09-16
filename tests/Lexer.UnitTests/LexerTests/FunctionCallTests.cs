namespace Lexemes.UnitTests;

public class FunctionCallTests
{
    [Fact]
    public void Can_tokenize_no_arg_function_call()
    {
        List<Token> actual = LexerHelper.Tokenize("f()");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("f")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.CloseParenthesis),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_function_call_with_one_argument()
    {
        List<Token> actual = LexerHelper.Tokenize("double(x)");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("double")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("x")),
            new Token(TokenType.CloseParenthesis),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_function_call_with_two_arguments()
    {
        List<Token> actual = LexerHelper.Tokenize("add(a, b)");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("add")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Comma),
            new Token(TokenType.Identifier, new TokenValue("b")),
            new Token(TokenType.CloseParenthesis),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_function_call_with_literal_arguments()
    {
        List<Token> actual = LexerHelper.Tokenize("add(3, 4)");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("add")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.IntLiteral, new TokenValue(3)),
            new Token(TokenType.Comma),
            new Token(TokenType.IntLiteral, new TokenValue(4)),
            new Token(TokenType.CloseParenthesis),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_nested_function_call()
    {
        List<Token> actual = LexerHelper.Tokenize("outer(inner())");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("outer")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("inner")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.CloseParenthesis),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_function_call_as_statement()
    {
        List<Token> actual = LexerHelper.Tokenize("greet();");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("greet")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_function_call_inside_print()
    {
        List<Token> actual = LexerHelper.Tokenize("print(double(5));");
        List<Token> expected =
        [
            new Token(TokenType.Print),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("double")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.IntLiteral, new TokenValue(5)),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }
}