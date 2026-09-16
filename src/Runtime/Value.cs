using System.Globalization;

namespace Runtime;

public class Value
{
    public static readonly Value Void = new(VoidValue.Value);
    private readonly object _value;

    public Value(int value)
    {
        _value = value;
    }

    public Value(object value)
    {
        _value = value;
    }

    public Value(string value)
    {
        _value = value;
    }

    public Value(double value)
    {
        _value = value;
    }

    public Value(bool value)
    {
        _value = value;
    }

    /// <summary>
    /// Возвращает тип значения.
    /// </summary>
    public ValueType GetValueType()
    {
        return _value switch
        {
            int => ValueType.Int,
            string => ValueType.String,
            double => ValueType.Float,
            bool => ValueType.Bool,
            _ => throw new InvalidOperationException($"Unexpected value {_value} of  type {_value.GetType()}"),
        };
    }

    /// <summary>
    /// Определяет, является ли значение целым числом.
    /// </summary>
    public bool IsInt()
    {
        return _value switch
        {
            int => true,
            _ => false,
        };
    }

    /// <summary>
    /// Определяет, является ли значение строкой.
    /// </summary>
    public bool IsString()
    {
        return _value switch
        {
            string => true,
            _ => false,
        };
    }

    /// <summary>
    /// Возвращает значение как строку либо бросает исключение.
    /// </summary>
    public string AsString()
    {
        return _value switch
        {
            string s => s,
            _ => throw new InvalidOperationException($"Value {_value} is not a string"),
        };
    }

    /// <summary>
    /// Определяет, является ли значение вещественным числом.
    /// </summary>
    public bool IsFloat()
    {
        return _value switch
        {
            double => true,
            _ => false,
        };
    }

    /// <summary>
    /// Возвращает значение как вещественное число либо бросает исключение.
    /// </summary>
    public double AsFloat()
    {
        return _value switch
        {
            double d => d,
            _ => throw new InvalidOperationException($"Value {_value} is not a float"),
        };
    }

    /// <summary>
    /// Определяет, является ли значение пустым (неопределённым).
    /// </summary>
    public bool IsVoid()
    {
        return _value switch
        {
            VoidValue => true,
            _ => false,
        };
    }

    /// <summary>
    /// Возвращает значение как целое число либо бросает исключение.
    /// </summary>
    public int AsInt()
    {
        return _value switch
        {
            int i => i,
            bool b => Convert.ToInt32(b),
            _ => throw new InvalidOperationException($"Value {_value} is not an integer"),
        };
    }

    /// <summary>
    /// Определяет, является ли значение булевым значением.
    /// </summary>
    public bool IsBool()
    {
        return _value switch
        {
            bool => true,
            _ => false,
        };
    }

    /// <summary>
    /// Возвращает значение как булевое значение либо бросает исключение.
    /// </summary>
    public bool AsBool()
    {
        return _value switch
        {
            bool b => b,
            _ => throw new InvalidOperationException($"Value {_value} is not an bool"),
        };
    }

    /// <summary>
    /// Печатает значение для отладки.
    /// </summary>
    public override string ToString()
    {
        return _value switch
        {
            int i => i.ToString(CultureInfo.InvariantCulture),
            string s => s,
            double d => d.ToString(CultureInfo.InvariantCulture),
            bool b => b ? "1" : "0",
            VoidValue v => v.ToString(),
            _ => throw new InvalidOperationException($"Unexpected value {_value} of type {_value.GetType()}"),
        };
    }

    public bool Equal(Value? other)
    {
        if (other == null)
        {
            return false;
        }

        if (GetValueType() != other.GetValueType())
        {
            return false;
        }

        return _value switch
        {
            int i => other.AsInt() == i,
            VoidValue => true,
            _ => throw new NotImplementedException(),
        };
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Value);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}