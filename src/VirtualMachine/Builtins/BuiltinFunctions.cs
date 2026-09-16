using System.Text;

using Runtime;

namespace VirtualMachine.Builtins;

public class BuiltinFunctions
{
    private readonly IEnvironment _environment;

    public BuiltinFunctions(IEnvironment environment)
    {
        _environment = environment;
    }

    public void Print(Value value)
    {
        if (value.IsBool())
        {
            if (value.AsBool())
            {
                _environment.Print(new Value(1));
            }
            else
            {
                _environment.Print(new Value(0));
            }
        }
        else
        {
            _environment.Print(value);
        }
    }

    public Value ReadI()
    {
        int v = _environment.ReadInt();
        return new Value(v);
    }

    public Value ReadF()
    {
        double v = _environment.ReadFloat();
        return new Value(v);
    }

    public Value ReadS()
    {
        string v = _environment.ReadString();
        return new Value(v);
    }

    public Value Length(Value value)
    {
        string str = value.AsString();

        int runeCount = str.EnumerateRunes().Count();

        return new Value(runeCount);
    }

    public Value Substr(Value source, Value start, Value length)
    {
        string str = source.AsString();
        int startIndex = start.AsInt();
        int requestedLength = length.AsInt();

        Rune[] runes = str.EnumerateRunes().ToArray();
        int totalRunes = runes.Length;

        if (startIndex < 0 || requestedLength < 0 || startIndex + requestedLength > totalRunes)
        {
            throw new InvalidOperationException(
                $"substr: invalid arguments (start={startIndex}, length={requestedLength}, string_length={totalRunes})"
            );
        }

        if (requestedLength == 0)
        {
            return new Value(string.Empty);
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < requestedLength; i++)
        {
            sb.Append(runes[startIndex + i].ToString());
        }

        return new Value(sb.ToString());
    }
}