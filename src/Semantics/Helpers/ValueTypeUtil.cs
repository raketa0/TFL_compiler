namespace Semantics.Helpers;

using ValueType = Runtime.ValueType;

public static class ValueTypeUtil
{
    public static bool AreCompatibleTypes(ValueType a, ValueType b)
    {
        return a == b;
    }
}