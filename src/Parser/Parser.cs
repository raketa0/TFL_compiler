using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

using Lexemes;

using Value = Runtime.Value;
using ValueType = Runtime.ValueType;

namespace Parsing;

public class Parser
{
    private readonly TokenStream _tokens;

    public Parser(string code)
    {
        _tokens = new TokenStream(code);
    }

    /// <summary>
    /// program =
    ///     "main",
    ///     block ;
    /// </summary>
    public ProgramNode ParseProgram()
    {
        List<FunctionDeclarationStatement> function = new();

        while (_tokens.Peek().Type == TokenType.Func)
        {
            function.Add(ParseFunctionDeclarationStatement());
        }

        Match(TokenType.Main);
        BlockStatement mainBlock = ParseBlock();
        return new ProgramNode(function, mainBlock);
    }

    /// <summary>
    /// block =
    ///     "{",
    ///     { statement },
    ///     "}" ;
    /// </summary>
    private BlockStatement ParseBlock()
    {
        Match(TokenType.OpenBrace);
        List<Statement> statements = new();

        while (_tokens.Peek().Type != TokenType.CloseBrace)
        {
            statements.Add(ParseStatement());
        }

        Match(TokenType.CloseBrace);

        return new BlockStatement(statements);
    }

    /// <summary>
    ///   statement =
    ///     var_decl
    ///   | const_decl
    ///   | assignment
    ///   | print_stmt
    ///   | read_stmt
    ///   | if_stmt
    ///   | while_stmt
    ///   | break_stmt
    ///   | continue_stmt
    ///   | return_stmt
    ///   | call_stmt
    ///   | block ;
    /// </summary>
    private Statement ParseStatement()
    {
        Token t = _tokens.Peek();
        return t.Type switch
        {
            TokenType.Var => ParseVarDecl(),
            TokenType.Const => ParseConstDecl(),
            TokenType.Identifier => ParseIdentifierStatement(),
            TokenType.Print => ParsePrintStatement(),
            TokenType.Read => ParseReadStatement(),
            TokenType.If => ParseIfElseStatement(),
            TokenType.While => ParseWhileStatement(),
            TokenType.Break => ParseBreakStatement(),
            TokenType.Continue => ParseContinueStatement(),
            TokenType.Return => ParseReturnStatement(),
            _ => ParseBlock(),
        };
    }

    /// <summary>
    /// var_decl =
    ///     "var",
    ///     identifier,
    ///     ":",
    ///     type,
    ///     ["=", expression],
    ///     ";" ;
    /// </summary>
    private VariableDeclarationStatement ParseVarDecl()
    {
        Match(TokenType.Var);
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.Colon);
        ValueType type = ParseType();
        if (_tokens.Peek().Type == TokenType.Assign)
        {
            Match(TokenType.Assign);
            Expression expr = ParseExpression();
            Match(TokenType.Semicolon);
            return new VariableDeclarationStatement(name, type, expr);
        }

        Match(TokenType.Semicolon);
        return new VariableDeclarationStatement(name, type, null);
    }

    /// <summary>
    /// const_decl =
    ///     "const",
    ///     identifier,
    ///     ":",
    ///     type,
    ///     "=",
    ///     expression,
    ///     ";" ;
    /// </summary>
    private ConstDeclarationStatement ParseConstDecl()
    {
        Match(TokenType.Const);
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.Colon);
        ValueType type = ParseType();
        Match(TokenType.Assign);
        Expression expr = ParseExpression();
        Match(TokenType.Semicolon);

        return new ConstDeclarationStatement(name, type, expr);
    }

    /// <summary>
    /// assignment =
    ///     identifier,
    ///     "=",
    ///     expression,
    ///     ";" ;
    /// </summary>
    private AssignmentStatement ParseAssignment()
    {
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.Assign);
        Expression expr = ParseExpression();
        Match(TokenType.Semicolon);

        return new AssignmentStatement(name, expr);
    }

    private FunctionCallStatement ParseFunctionCallStatement()
    {
        FunctionCallExpression expr = ParseFunctionCallExpression();
        Match(TokenType.Semicolon);

        return new FunctionCallStatement(expr);
    }

    private Statement ParseIdentifierStatement()
    {
        Token next = _tokens.Peek(1);

        return next.Type switch
        {
            TokenType.Assign => ParseAssignment(),
            TokenType.OpenParenthesis => ParseFunctionCallStatement(),
            _ => throw new Exception("Expected assignment or function call"),
        };
    }

    /// <summary>
    /// read_stmt =
    ///     "read",
    ///     "(",
    ///     identifier,
    ///     ")",
    ///     ";" ;
    /// </summary>
    private ReadStatement ParseReadStatement()
    {
        Match(TokenType.Read);
        Match(TokenType.OpenParenthesis);
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.CloseParenthesis);
        Match(TokenType.Semicolon);

        return new ReadStatement(name);
    }

    /// <summary>
    /// print_stmt =
    ///     "print",
    ///     "(",
    ///     expression,
    ///     ")",
    ///     ";" ;
    /// </summary>
    private PrintStatement ParsePrintStatement()
    {
        Match(TokenType.Print);
        Match(TokenType.OpenParenthesis);
        Expression expression = ParseExpression();
        Match(TokenType.CloseParenthesis);
        Match(TokenType.Semicolon);

        return new PrintStatement(expression);
    }

    /// <summary>
    /// if_stmt =
    ///          "if",
    ///          "(",
    ///          expression,
    ///          ")",
    ///          block,
    ///          [ "else", ( block | if_stmt ) ] ;
    /// </summary>
    private IfElseStatement ParseIfElseStatement()
    {
        Match(TokenType.If);
        Match(TokenType.OpenParenthesis);
        Expression condition = ParseExpression();
        Match(TokenType.CloseParenthesis);
        BlockStatement statement = ParseBlock();
        Statement? elseBranch = null;
        if (_tokens.Peek().Type == TokenType.Else)
        {
            Match(TokenType.Else);
            if (_tokens.Peek().Type == TokenType.If)
            {
                elseBranch = ParseIfElseStatement();
            }
            else
            {
                elseBranch = ParseBlock();
            }
        }

        return new IfElseStatement(condition, statement, elseBranch);
    }

    /// <summary>
    /// while_stmt =
    ///    "while",
    ///    "(",
    ///    expression,
    ///    ")",
    ///    block ;
    /// </summary>
    private WhileStatement ParseWhileStatement()
    {
        Match(TokenType.While);
        Match(TokenType.OpenParenthesis);
        Expression condition = ParseExpression();
        Match(TokenType.CloseParenthesis);
        BlockStatement block = ParseBlock();

        return new WhileStatement(condition, block);
    }

    /// <summary>
    /// break_stmt =
    ///        "break",
    ///        ";" ;
    /// </summary>
    private BreakStatement ParseBreakStatement()
    {
        Match(TokenType.Break);
        Match(TokenType.Semicolon);

        return new BreakStatement();
    }

    /// <summary>
    /// continue_stmt =
    ///    "continue",
    ///    ";" ;
    /// </summary>
    private ContinueStatement ParseContinueStatement()
    {
        Match(TokenType.Continue);
        Match(TokenType.Semicolon);

        return new ContinueStatement();
    }

    private ReturnStatement ParseReturnStatement()
    {
        Match(TokenType.Return);

        Expression? expr = null;
        if (_tokens.Peek().Type != TokenType.Semicolon)
        {
            expr = ParseExpression();
        }

        Match(TokenType.Semicolon);
        return new ReturnStatement(expr);
    }

    private FunctionDeclarationStatement ParseFunctionDeclarationStatement()
    {
        Match(TokenType.Func);
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.OpenParenthesis);
        List<AbstractParametrStatement> parameters = new List<AbstractParametrStatement>();
        if (_tokens.Peek().Type != TokenType.CloseParenthesis)
        {
            parameters.Add(ParseParametrDeclaration());
            while (_tokens.Peek().Type == TokenType.Comma)
            {
                Match(TokenType.Comma);
                parameters.Add(ParseParametrDeclaration());
            }
        }

        Match(TokenType.CloseParenthesis);
        Match(TokenType.Colon);
        ValueType type = ParseReturnType();
        BlockStatement block = ParseBlock();

        return new FunctionDeclarationStatement(name, parameters, type, block);
    }

    private ParametrDeclaration ParseParametrDeclaration()
    {
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.Colon);
        ValueType type = ParseType();

        return new ParametrDeclaration(name, type);
    }

    private ValueType ParseReturnType()
    {
        try
        {
            return ParseType();
        }
        catch
        {
            Match(TokenType.Void);
            return ValueType.Void;
        }
    }

    /// <summary>
    /// expression = logical_or ;
    /// </summary>
    private Expression ParseExpression()
    {
        return ParseLogicalOr();
    }

    /// <summary>
    /// logical_or =
    ///        logical_and,
    ///        { "||", logical_and } ;
    /// </summary>
    private Expression ParseLogicalOr()
    {
        Expression expr = ParseLogicalAnd();
        while (true)
        {
            switch (_tokens.Peek().Type)
            {
                case TokenType.LogicalOr:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.LogicalOr, ParseLogicalAnd());
                    break;
                default:
                    return expr;
            }
        }
    }

    /// <summary>
    ///    logical_and =
    ///        equality,
    ///        { "&&", equality } ;
    /// </summary>
    private Expression ParseLogicalAnd()
    {
        Expression expr = ParseEquality();
        while (true)
        {
            switch (_tokens.Peek().Type)
            {
                case TokenType.LogicalAnd:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.LogicalAnd, ParseEquality());
                    break;
                default:
                    return expr;
            }
        }
    }

    /// <summary>
    ///    equality =
    ///        relational,
    ///        { ( "==" | "!=" ), relational } ;
    /// </summary>
    private Expression ParseEquality()
    {
        Expression expr = ParseRational();
        while (true)
        {
            switch (_tokens.Peek().Type)
            {
                case TokenType.Equal:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Equal, ParseAdditive());
                    break;
                case TokenType.NotEqual:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.NotEqual, ParseAdditive());
                    break;
                default:
                    return expr;
            }
        }
    }

    /// <summary>
    /// relational =
    ///        additive,
    ///        { ( "<" | ">" | "<=" | ">=" ), additive } ;
    /// </summary>
    private Expression ParseRational()
    {
        Expression expr = ParseAdditive();
        while (true)
        {
            switch (_tokens.Peek().Type)
            {
                case TokenType.LessThan:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.LessThan, ParseAdditive());
                    break;
                case TokenType.LessThanOrEqual:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.LessThanOrEqual, ParseAdditive());
                    break;
                case TokenType.GreaterThan:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.GreaterThan, ParseAdditive());
                    break;
                case TokenType.GreaterThanOrEqual:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.GreaterThanOrEqual, ParseAdditive());
                    break;
                default:
                    return expr;
            }
        }
    }

    /// <summary>
    /// additive =
    ///     multiplicative,
    ///     { ( "+" | "-" ), multiplicative };
    /// </summary>
    private Expression ParseAdditive()
    {
        Expression expr = ParseMultiplicative();
        while (true)
        {
            switch (_tokens.Peek().Type)
            {
                case TokenType.Plus:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Add, ParseMultiplicative());
                    break;
                case TokenType.Minus:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Subtract, ParseMultiplicative());
                    break;
                default:
                    return expr;
            }
        }
    }

    /// <summary>
    /// multiplicative =
    ///     unary,
    ///     { ( "*" | "/" | "%" ), unary };
    /// </summary>
    private Expression ParseMultiplicative()
    {
        Expression expr = ParseUnary();
        while (true)
        {
            switch (_tokens.Peek().Type)
            {
                case TokenType.Multiply:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Multiply, ParseUnary());
                    break;
                case TokenType.Divide:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Divide, ParseUnary());
                    break;
                case TokenType.Module:
                    _tokens.Advance();
                    expr = new BinaryOperationExpression(expr, BinaryOperation.Module, ParseUnary());
                    break;
                default:
                    return expr;
            }
        }
    }

    /// <summary>
    /// unary =
    ///        "-", unary
    ///        | "!", unary
    ///        | primary ;
    /// </summary>
    private Expression ParseUnary()
    {
        if (_tokens.Peek().Type == TokenType.Minus)
        {
            Match(TokenType.Minus);
            return new UnaryOperationExpression(UnaryOperation.Minus, ParseUnary());
        }

        if (_tokens.Peek().Type == TokenType.LogicalNot)
        {
            Match(TokenType.LogicalNot);
            return new UnaryOperationExpression(UnaryOperation.LogicalNot, ParseUnary());
        }

        return ParsePrimary();
    }

    /// <summary>
    /// primary =
    ///     literal
    ///     | identifier
    ///     | "(",
    ///     expression,
    ///     ")" ;
    /// </summary>
    private Expression ParsePrimary()
    {
        Token t = _tokens.Peek();
        switch (t.Type)
        {
            case TokenType.IntLiteral:
                _tokens.Advance();
                return new LiteralExpression(new Value(t.Value!.ToInt()));
            case TokenType.FloatLiteral:
                _tokens.Advance();
                return new LiteralExpression(new Value(t.Value!.ToFloat()));
            case TokenType.StringLiteral:
                _tokens.Advance();
                return new LiteralExpression(new Value(t.Value!.ToString()));
            case TokenType.BoolLiteral:
                _tokens.Advance();
                return new LiteralExpression(new Value(t.Value!.ToBool()));
            case TokenType.Identifier:
                if (_tokens.Peek(1).Type == TokenType.OpenParenthesis)
                {
                    return ParseFunctionCallExpression();
                }

                string name = Match(TokenType.Identifier).Value!.ToString();
                return new VariableExpression(name);
            case TokenType.OpenParenthesis:
                _tokens.Advance();
                Expression expression = ParseExpression();
                Match(TokenType.CloseParenthesis);
                return expression;
            case TokenType.Length:
                _tokens.Advance();
                Match(TokenType.OpenParenthesis);
                Expression lengthArg = ParseExpression();
                Match(TokenType.CloseParenthesis);
                return new LengthExpression(lengthArg);
            case TokenType.Substr:
                _tokens.Advance();
                Match(TokenType.OpenParenthesis);
                Expression substrSource = ParseExpression();
                Match(TokenType.Comma);
                Expression substrStart = ParseExpression();
                Match(TokenType.Comma);
                Expression substrLength = ParseExpression();
                Match(TokenType.CloseParenthesis);
                return new SubstrExpression(substrSource, substrStart, substrLength);
            default:
                throw new UnexpectedLexemeException(
                    t, expected:
                    [
                        TokenType.IntLiteral,
                        TokenType.FloatLiteral,
                        TokenType.StringLiteral,
                        TokenType.BoolLiteral,
                        TokenType.Identifier,
                        TokenType.OpenParenthesis,
                    ]
                );
        }
    }

    private FunctionCallExpression ParseFunctionCallExpression()
    {
        string name = Match(TokenType.Identifier).Value!.ToString();
        Match(TokenType.OpenParenthesis);
        List<Expression> arguments = new();
        if (_tokens.Peek().Type != TokenType.CloseParenthesis)
        {
            arguments.Add(ParseExpression());
            while (_tokens.Peek().Type == TokenType.Comma)
            {
                Match(TokenType.Comma);
                arguments.Add(ParseExpression());
            }
        }

        Match(TokenType.CloseParenthesis);
        return new FunctionCallExpression(name, arguments);
    }

    private Token Match(TokenType expected)
    {
        Token t = _tokens.Peek();

        if (t.Type != expected)
        {
            throw new UnexpectedLexemeException(t, expected);
        }

        _tokens.Advance();

        return t;
    }

    private ValueType ParseType()
    {
        Token t = _tokens.Peek();

        ValueType result = t.Type switch
        {
            TokenType.IntegerType => ValueType.Int,
            TokenType.FloatType => ValueType.Float,
            TokenType.StringType => ValueType.String,
            TokenType.BooleanType => ValueType.Bool,
            _ => throw new Exception($"Expected types 'int', 'float', 'string', got {t.Type}"),
        };

        _tokens.Advance();

        return result;
    }
}