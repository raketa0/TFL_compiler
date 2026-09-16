using System.Runtime.CompilerServices;

namespace Runtime;

public class ValueType
{
    /// <summary>
    /// Значение отсутствует.
    /// </summary>
    public static readonly ValueType Void = new("void" );

    /// <summary>
    /// Строковое значение.
    /// </summary>
    public static readonly ValueType String = new("string");

    /// <summary>
    /// Вещественное значение.
    /// </summary>
    public static readonly ValueType Float = new("float");

    /// <summary>
    /// Целочисленное значение.
    /// </summary>
    public static readonly ValueType Int = new("int");

    /// <summary>
    /// Булевое значение
    /// </summary>
    public static readonly ValueType Bool = new("bool");

    private readonly string _name;

    public ValueType(string name)
    {
        _name = name;
    }

    public static bool operator ==(ValueType a, ValueType b) => a.Equals(b);

    public static bool operator !=(ValueType a, ValueType b) => !a.Equals(b);

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj);
    }

    public override int GetHashCode()
    {
        return RuntimeHelpers.GetHashCode(this);
    }

    public override string ToString()
    {
        return _name;
    }
}