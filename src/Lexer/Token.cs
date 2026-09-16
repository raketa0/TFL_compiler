using System.Text;

namespace Lexemes;

public class Token
{
    public Token(TokenType type)
    {
        Type = type;
    }

    public Token(TokenType type, TokenValue value)
    {
        Type = type;
        Value = value;
    }

    public TokenType Type { get; }

    public TokenValue? Value { get; }

    public override bool Equals(object? obj)
    {
        if (obj is Token other)
        {
            return Type == other.Type && Equals(Value, other.Value);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Value);
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(Type);
        if (Value != null)
        {
            sb.Append($" ({Value})");
        }

        return sb.ToString();
    }
}