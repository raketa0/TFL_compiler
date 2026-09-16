using System.Globalization;

using Runtime;

namespace VirtualMachine;

public class ConsoleEnvironment : IEnvironment
{
    public int ReadInt()
    {
        string? input = Console.ReadLine();

        if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
        {
            return value;
        }

        throw new InvalidOperationException($"Invalid integer input: {input}");
    }

    public double ReadFloat()
    {
        string? input = Console.ReadLine();

        if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
        {
            return value;
        }

        throw new InvalidOperationException($"Invalid float input: {input}");
    }

    public string ReadString()
    {
        return Console.ReadLine() ?? "";
    }

    public void Print(Value value)
    {
        if (value.IsInt())
        {
            Console.Write(value.AsInt().ToString(CultureInfo.InvariantCulture));
        }
        else if (value.IsFloat())
        {
            Console.Write(value.AsFloat().ToString(CultureInfo.InvariantCulture));
        }
        else if (value.IsString())
        {
            Console.Write(value.AsString());
        }
        else
        {
            Console.Write(value.ToString());
        }
    }
}