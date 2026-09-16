using System.Globalization;

namespace Lexemes;

public class TokenValue
{
    private readonly object _value;

    public TokenValue(string value)
    {
        _value = value;
    }

    public TokenValue(int value)
    {
        _value = value;
    }

    public TokenValue(double value)
    {
        _value = value;
    }

    public TokenValue(bool value)
    {
        _value = value;
    }

    public int ToInt()
    {
        return _value switch
        {
            int i => i,
            string s => int.Parse(s, CultureInfo.InvariantCulture),
            _ => throw new NotImplementedException(),
        };
    }

    public double ToFloat()
    {
        return _value switch
        {
            double d => d,
            string s => double.Parse(s, CultureInfo.InvariantCulture),
            _ => throw new NotImplementedException(),
        };
    }

    public override string ToString()
    {
        return _value switch
        {
            string s => s,
            int i => i.ToString(),
            double d => d.ToString(CultureInfo.InvariantCulture),
            _ => throw new NotImplementedException(),
        };
    }

    public bool ToBool()
    {
        return _value switch
        {
            bool b => b,
            _ => throw new NotImplementedException(),
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TokenValue other)
        {
            return false;
        }

        return _value.Equals(other._value);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}