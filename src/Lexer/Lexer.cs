using System.Globalization;
using System.Text;

namespace Lexemes;

public class Lexer
{
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "main", TokenType.Main },
        { "var", TokenType.Var },
        { "const", TokenType.Const },
        { "int", TokenType.IntegerType },
        { "float", TokenType.FloatType },
        { "string", TokenType.StringType },
        { "bool", TokenType.BooleanType },
        { "read", TokenType.Read },
        { "print", TokenType.Print },
        { "length", TokenType.Length },
        { "substr", TokenType.Substr },
        { "if", TokenType.If },
        { "else", TokenType.Else },
        { "continue", TokenType.Continue },
        { "break", TokenType.Break },
        { "return", TokenType.Return },
        { "while", TokenType.While },
        { "func", TokenType.Func },
        { "void", TokenType.Void },
        { "true", TokenType.BoolLiteral },
        { "false", TokenType.BoolLiteral },
    };

    private static readonly Dictionary<char, char> SimpleEscapes = new()
    {
        { 'n', '\n' }, { 't', '\t' }, { '"', '\"' }, { '\\', '\\' },
    };

    private readonly TextScanner _scanner;

    public Lexer(string str)
    {
        _scanner = new TextScanner(str);
    }

    public Token ParseToken()
    {
        SkipWhiteSpacesAndComments();

        if (_scanner.IsEnd())
        {
            return new Token(TokenType.EndOfFile);
        }

        char c = _scanner.Peek();

        if (char.IsLetter(c) || c == '_')
        {
            return ParseIdentifierOrKeyword();
        }

        if (char.IsDigit(c))
        {
            return ParseNumericLiteral();
        }

        if (c == '"')
        {
            return ParseStringLiteral();
        }

        switch (c)
        {
            case '+':
                _scanner.Advance();
                return new Token(TokenType.Plus);
            case '-':
                _scanner.Advance();
                return new Token(TokenType.Minus);
            case '*':
                _scanner.Advance();
                return new Token(TokenType.Multiply);
            case '/':
                _scanner.Advance();
                return new Token(TokenType.Divide);
            case '%':
                _scanner.Advance();
                return new Token(TokenType.Module);
            case '=':
                if (_scanner.Peek(1) == '=')
                {
                    _scanner.Advance();
                    _scanner.Advance();
                    return new Token(TokenType.Equal);
                }

                _scanner.Advance();
                return new Token(TokenType.Assign);
            case '!':
                if (_scanner.Peek(1) == '=')
                {
                    _scanner.Advance();
                    _scanner.Advance();
                    return new Token(TokenType.NotEqual);
                }

                _scanner.Advance();
                return new Token(TokenType.LogicalNot);

            case ',':
                _scanner.Advance();
                return new Token(TokenType.Comma);
            case ';':
                _scanner.Advance();
                return new Token(TokenType.Semicolon);
            case ':':
                _scanner.Advance();
                return new Token(TokenType.Colon);

            case '{':
                _scanner.Advance();
                return new Token(TokenType.OpenBrace);
            case '}':
                _scanner.Advance();
                return new Token(TokenType.CloseBrace);
            case '(':
                _scanner.Advance();
                return new Token(TokenType.OpenParenthesis);
            case ')':
                _scanner.Advance();
                return new Token(TokenType.CloseParenthesis);
            case '&':
                if (_scanner.Peek(1) == '&')
                {
                    _scanner.Advance();
                    _scanner.Advance();

                    return new Token(TokenType.LogicalAnd);
                }

                _scanner.Advance();
                return new Token(TokenType.Error, new TokenValue("&"));
            case '|':
                if (_scanner.Peek(1) == '|')
                {
                    _scanner.Advance();
                    _scanner.Advance();

                    return new Token(TokenType.LogicalOr);
                }

                _scanner.Advance();
                return new Token(TokenType.Error, new TokenValue("|"));
            case '<':
                if (_scanner.Peek(1) == '=')
                {
                    _scanner.Advance();
                    _scanner.Advance();

                    return new Token(TokenType.LessThanOrEqual);
                }

                _scanner.Advance();
                return new Token(TokenType.LessThan);
            case '>':
                if (_scanner.Peek(1) == '=')
                {
                    _scanner.Advance();
                    _scanner.Advance();

                    return new Token(TokenType.GreaterThanOrEqual);
                }

                _scanner.Advance();
                return new Token(TokenType.GreaterThan);
        }

        _scanner.Advance();
        return new Token(TokenType.Error, new TokenValue(c.ToString()));
    }

    private Token ParseIdentifierOrKeyword()
    {
        char first = _scanner.Peek();

        if (!(char.IsLetter(first) || first == '_'))
        {
            return new Token(TokenType.Error, new TokenValue(first.ToString()));
        }

        string value = "";
        value += first;
        _scanner.Advance();

        while (char.IsLetterOrDigit(_scanner.Peek()) || _scanner.Peek() == '_')
        {
            value += _scanner.Peek();
            _scanner.Advance();
        }

        if (Keywords.TryGetValue(value, out TokenType type))
        {
            if (type == TokenType.BoolLiteral)
            {
                return new Token(type, new TokenValue(value == "true"));
            }

            return new Token(type);
        }

        return new Token(TokenType.Identifier, new TokenValue(value));
    }

    private Token ParseNumericLiteral()
    {
        string value = "";

        while (char.IsDigit(_scanner.Peek()))
        {
            value += _scanner.Peek();
            _scanner.Advance();
        }

        if (_scanner.Peek() == '.')
        {
            value += '.';
            _scanner.Advance();

            if (!char.IsDigit(_scanner.Peek()))
            {
                return new Token(TokenType.Error, new TokenValue(value));
            }

            while (char.IsDigit(_scanner.Peek()))
            {
                value += _scanner.Peek();
                _scanner.Advance();
            }

            if (double.TryParse(value, CultureInfo.InvariantCulture, out double f))
            {
                return new Token(TokenType.FloatLiteral, new TokenValue(f));
            }

            return new Token(TokenType.Error, new TokenValue(value));
        }

        if (int.TryParse(value, out int i))
        {
            return new Token(TokenType.IntLiteral, new TokenValue(i));
        }

        return new Token(TokenType.Error, new TokenValue(value));
    }

    private Token ParseStringLiteral()
    {
        StringBuilder value = new();
        bool hasError = false;

        _scanner.Advance();

        while (_scanner.Peek() != '"')
        {
            if (_scanner.IsEnd())
            {
                return new Token(TokenType.Error, new TokenValue(value.ToString()));
            }

            if (_scanner.Peek() == '\\')
            {
                if (!DecodeEscapeSequence(value))
                {
                    hasError = true;
                    value.Append('\\');
                }
            }
            else
            {
                value.Append(_scanner.Peek());
                _scanner.Advance();
            }
        }

        _scanner.Advance();

        if (hasError)
        {
            return new Token(TokenType.Error, new TokenValue(value.ToString()));
        }

        return new Token(TokenType.StringLiteral, new TokenValue(value.ToString()));
    }

    private bool DecodeEscapeSequence(StringBuilder value)
    {
        _scanner.Advance();

        char c = _scanner.Peek();

        if (SimpleEscapes.TryGetValue(c, out char decoded))
        {
            _scanner.Advance();
            value.Append(decoded);
            return true;
        }

        return false;
    }

    private void SkipWhiteSpacesAndComments()
    {
        while (true)
        {
            SkipWhiteSpace();

            if (!TryParseSingleLineComment())
            {
                break;
            }
        }
    }

    private void SkipWhiteSpace()
    {
        while (char.IsWhiteSpace(_scanner.Peek()))
        {
            _scanner.Advance();
        }
    }

    private bool TryParseSingleLineComment()
    {
        if (_scanner.Peek() == '#')
        {
            while (!_scanner.IsEnd() && _scanner.Peek() != '\n')
            {
                _scanner.Advance();
            }

            return true;
        }

        return false;
    }
}