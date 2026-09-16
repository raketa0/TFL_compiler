using System.Globalization;
using System.Text;

using Runtime;

using VirtualMachine;

namespace Tests.TestLibrary.TestDoubles;

/// <summary>
/// Имитирует средства ввода-вывода для тестов.
/// </summary>
public class FakeEnvironment : IEnvironment
{
    private readonly Queue<int> _intInput = new();
    private readonly Queue<double> _floatInput = new();
    private readonly Queue<string> _stringInput = new();
    private readonly StringBuilder _output = new();

    public string Output => _output.ToString();

    // Для обратной совместимости с существующими тестами
    public void AddInput(params int[] values)
    {
        foreach (int val in values)
        {
            _intInput.Enqueue(val);
        }
    }

    public void AddFloatInput(params double[] values)
    {
        foreach (double val in values)
        {
            _floatInput.Enqueue(val);
        }
    }

    public void AddStringInput(params string[] values)
    {
        foreach (string val in values)
        {
            _stringInput.Enqueue(val);
        }
    }

    public int ReadInt()
    {
        if (_intInput.TryDequeue(out int value))
        {
            return value;
        }

        throw new InvalidOperationException("No integer input available");
    }

    public double ReadFloat()
    {
        if (_floatInput.TryDequeue(out double value))
        {
            return value;
        }

        throw new InvalidOperationException("No float input available");
    }

    public string ReadString()
    {
        if (_stringInput.TryDequeue(out string? value))
        {
            return value;
        }

        throw new InvalidOperationException("No string input available");
    }

    public void PrintInt(int value)
    {
        _output.Append(value.ToString(CultureInfo.InvariantCulture));
    }

    public void Print(Value value)
    {
        _output.Append(value.ToString());
    }
}