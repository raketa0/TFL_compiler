namespace Lexemes.UnitTests;

public class OperatorTests
{
    [Theory]
    [MemberData(nameof(GetArithmeticOperators))]
    public void Can_tokenize_arithmetic_operators(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetArithmeticOperators()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "+", [new Token(TokenType.Plus)] },
            { "-", [new Token(TokenType.Minus)] },
            { "*", [new Token(TokenType.Multiply)] },
            { "/", [new Token(TokenType.Divide)] },
            { "%", [new Token(TokenType.Module)] },
            {
                "+ - * / %",
                [
                    new Token(TokenType.Plus),
                    new Token(TokenType.Minus),
                    new Token(TokenType.Multiply),
                    new Token(TokenType.Divide),
                    new Token(TokenType.Module),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetComparisonOperators))]
    public void Can_tokenize_comparison_operators(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetComparisonOperators()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "==", [new Token(TokenType.Equal)] },
            { "!=", [new Token(TokenType.NotEqual)] },
            {
                "== !=",
                [
                    new Token(TokenType.Equal),
                    new Token(TokenType.NotEqual),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetAssignOperator))]
    public void Can_tokenize_assign_operator(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetAssignOperator()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "=", [new Token(TokenType.Assign)] },

            // = должен отличаться от ==
            {
                "= ==",
                [
                    new Token(TokenType.Assign),
                    new Token(TokenType.Equal),
                ]
            },
        };
    }

    [Fact]
    public void Minus_before_number_produces_two_tokens()
    {
        // унарный минус — отдельный Minus токен, не часть числового литерала
        List<Token> actual = LexerHelper.Tokenize("-42");
        List<Token> expected =
        [
            new Token(TokenType.Minus),
            new Token(TokenType.IntLiteral, new TokenValue(42)),
        ];
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetRelationalOperators))]
    public void Can_tokenize_relational_operators(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetRelationalOperators()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "<",  [new Token(TokenType.LessThan)] },
            { "<=", [new Token(TokenType.LessThanOrEqual)] },
            { ">",  [new Token(TokenType.GreaterThan)] },
            { ">=", [new Token(TokenType.GreaterThanOrEqual)] },
            {
                "< <= > >=",
                [
                    new Token(TokenType.LessThan),
                    new Token(TokenType.LessThanOrEqual),
                    new Token(TokenType.GreaterThan),
                    new Token(TokenType.GreaterThanOrEqual),
                ]
            },
            {
                "< =",
                [
                    new Token(TokenType.LessThan),
                    new Token(TokenType.Assign),
                ]
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetLogicalOperators))]
    public void Can_tokenize_logical_operators(string code, List<Token> expected)
    {
        List<Token> actual = LexerHelper.Tokenize(code);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<string, List<Token>> GetLogicalOperators()
    {
        return new TheoryData<string, List<Token>>()
        {
            { "&&", [new Token(TokenType.LogicalAnd)] },
            { "||", [new Token(TokenType.LogicalOr)] },
            { "!",  [new Token(TokenType.LogicalNot)] },
            {
                "&& || !",
                [
                    new Token(TokenType.LogicalAnd),
                    new Token(TokenType.LogicalOr),
                    new Token(TokenType.LogicalNot),
                ]
            },
            {
                "! !=",
                [
                    new Token(TokenType.LogicalNot),
                    new Token(TokenType.NotEqual),
                ]
            },
        };
    }
}