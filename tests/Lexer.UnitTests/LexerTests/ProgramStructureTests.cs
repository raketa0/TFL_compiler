namespace Lexemes.UnitTests;

public class ProgramStructureTests
{
    [Fact]
    public void Can_tokenize_var_declaration()
    {
        List<Token> actual = LexerHelper.Tokenize("var x : int = 42;");
        List<Token> expected =
        [
            new Token(TokenType.Var),
            new Token(TokenType.Identifier, new TokenValue("x")),
            new Token(TokenType.Colon),
            new Token(TokenType.IntegerType),
            new Token(TokenType.Assign),
            new Token(TokenType.IntLiteral, new TokenValue(42)),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_const_declaration()
    {
        List<Token> actual = LexerHelper.Tokenize("const y : int = 0;");
        List<Token> expected =
        [
            new Token(TokenType.Const),
            new Token(TokenType.Identifier, new TokenValue("y")),
            new Token(TokenType.Colon),
            new Token(TokenType.IntegerType),
            new Token(TokenType.Assign),
            new Token(TokenType.IntLiteral, new TokenValue(0)),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_read_statement()
    {
        List<Token> actual = LexerHelper.Tokenize("read(x);");
        List<Token> expected =
        [
            new Token(TokenType.Read),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("x")),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_print_statement()
    {
        List<Token> actual = LexerHelper.Tokenize("print(a);");
        List<Token> expected =
        [
            new Token(TokenType.Print),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_arithmetic_expression()
    {
        List<Token> actual = LexerHelper.Tokenize("a + b * c - d / e % f");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Plus),
            new Token(TokenType.Identifier, new TokenValue("b")),
            new Token(TokenType.Multiply),
            new Token(TokenType.Identifier, new TokenValue("c")),
            new Token(TokenType.Minus),
            new Token(TokenType.Identifier, new TokenValue("d")),
            new Token(TokenType.Divide),
            new Token(TokenType.Identifier, new TokenValue("e")),
            new Token(TokenType.Module),
            new Token(TokenType.Identifier, new TokenValue("f")),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_comparison_expression()
    {
        List<Token> actual = LexerHelper.Tokenize("a == b != c");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Equal),
            new Token(TokenType.Identifier, new TokenValue("b")),
            new Token(TokenType.NotEqual),
            new Token(TokenType.Identifier, new TokenValue("c")),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_assignment_statement()
    {
        List<Token> actual = LexerHelper.Tokenize("x = 10;");
        List<Token> expected =
        [
            new Token(TokenType.Identifier, new TokenValue("x")),
            new Token(TokenType.Assign),
            new Token(TokenType.IntLiteral, new TokenValue(10)),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_main_block()
    {
        List<Token> actual = LexerHelper.Tokenize(
            """
            main {
                var a : int = 10;
                print(a);
            }
            """);
        List<Token> expected =
        [
            new Token(TokenType.Main),
            new Token(TokenType.OpenBrace),
            new Token(TokenType.Var),
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Colon),
            new Token(TokenType.IntegerType),
            new Token(TokenType.Assign),
            new Token(TokenType.IntLiteral, new TokenValue(10)),
            new Token(TokenType.Semicolon),
            new Token(TokenType.Print),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Semicolon),
            new Token(TokenType.CloseBrace),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_float_var_declaration()
    {
        List<Token> actual = LexerHelper.Tokenize("var x : float = 1.5;");
        List<Token> expected =
        [
            new Token(TokenType.Var),
            new Token(TokenType.Identifier, new TokenValue("x")),
            new Token(TokenType.Colon),
            new Token(TokenType.FloatType),
            new Token(TokenType.Assign),
            new Token(TokenType.FloatLiteral, new TokenValue(1.5)),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_string_var_declaration()
    {
        List<Token> actual = LexerHelper.Tokenize("var s : string = \"hello\";");
        List<Token> expected =
        [
            new Token(TokenType.Var),
            new Token(TokenType.Identifier, new TokenValue("s")),
            new Token(TokenType.Colon),
            new Token(TokenType.StringType),
            new Token(TokenType.Assign),
            new Token(TokenType.StringLiteral, new TokenValue("hello")),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_length_call()
    {
        List<Token> actual = LexerHelper.Tokenize("length(s)");
        List<Token> expected =
        [
            new Token(TokenType.Length),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("s")),
            new Token(TokenType.CloseParenthesis),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_substr_call()
    {
        List<Token> actual = LexerHelper.Tokenize("substr(s, 0, 3)");
        List<Token> expected =
        [
            new Token(TokenType.Substr),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("s")),
            new Token(TokenType.Comma),
            new Token(TokenType.IntLiteral, new TokenValue(0)),
            new Token(TokenType.Comma),
            new Token(TokenType.IntLiteral, new TokenValue(3)),
            new Token(TokenType.CloseParenthesis),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_program_with_comments()
    {
        List<Token> actual = LexerHelper.Tokenize(
            """
            main {
                # читаем число
                read(x);
                print(x); # выводим
            }
            """);
        List<Token> expected =
        [
            new Token(TokenType.Main),
            new Token(TokenType.OpenBrace),
            new Token(TokenType.Read),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("x")),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Semicolon),
            new Token(TokenType.Print),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("x")),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Semicolon),
            new Token(TokenType.CloseBrace),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_func_declaration_header()
    {
        List<Token> actual = LexerHelper.Tokenize("func add(a: int, b: int): int");
        List<Token> expected =
        [
            new Token(TokenType.Func),
            new Token(TokenType.Identifier, new TokenValue("add")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Colon),
            new Token(TokenType.IntegerType),
            new Token(TokenType.Comma),
            new Token(TokenType.Identifier, new TokenValue("b")),
            new Token(TokenType.Colon),
            new Token(TokenType.IntegerType),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Colon),
            new Token(TokenType.IntegerType),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_void_func_declaration_header()
    {
        List<Token> actual = LexerHelper.Tokenize("func greet(): void");
        List<Token> expected =
        [
            new Token(TokenType.Func),
            new Token(TokenType.Identifier, new TokenValue("greet")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Colon),
            new Token(TokenType.Void),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_return_statement()
    {
        List<Token> actual = LexerHelper.Tokenize("return a + b;");
        List<Token> expected =
        [
            new Token(TokenType.Return),
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Plus),
            new Token(TokenType.Identifier, new TokenValue("b")),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_bare_return_statement()
    {
        List<Token> actual = LexerHelper.Tokenize("return;");
        List<Token> expected =
        [
            new Token(TokenType.Return),
            new Token(TokenType.Semicolon),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_full_func_declaration()
    {
        List<Token> actual = LexerHelper.Tokenize(
            """
            func add(a: int, b: int): int {
                return a + b;
            }
            """);
        List<Token> expected =
        [
            new Token(TokenType.Func),
            new Token(TokenType.Identifier, new TokenValue("add")),
            new Token(TokenType.OpenParenthesis),
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Colon),
            new Token(TokenType.IntegerType),
            new Token(TokenType.Comma),
            new Token(TokenType.Identifier, new TokenValue("b")),
            new Token(TokenType.Colon),
            new Token(TokenType.IntegerType),
            new Token(TokenType.CloseParenthesis),
            new Token(TokenType.Colon),
            new Token(TokenType.IntegerType),
            new Token(TokenType.OpenBrace),
            new Token(TokenType.Return),
            new Token(TokenType.Identifier, new TokenValue("a")),
            new Token(TokenType.Plus),
            new Token(TokenType.Identifier, new TokenValue("b")),
            new Token(TokenType.Semicolon),
            new Token(TokenType.CloseBrace),
        ];
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Can_tokenize_unicode_string_literal()
    {
        List<Token> actual = LexerHelper.Tokenize("var s : string = \"Hello, 🚀\";");

        List<Token> expected =
        [
            new Token(TokenType.Var),
        new Token(TokenType.Identifier, new TokenValue("s")),
        new Token(TokenType.Colon),
        new Token(TokenType.StringType),
        new Token(TokenType.Assign),
        new Token(TokenType.StringLiteral, new TokenValue("Hello, 🚀")),
        new Token(TokenType.Semicolon),
    ];

        Assert.Equal(expected, actual);
    }
}