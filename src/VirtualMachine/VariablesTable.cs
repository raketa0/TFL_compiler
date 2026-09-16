using Runtime;

namespace VirtualMachine;

public class VariablesTable
{
    private readonly VariablesTable? _parent;
    private readonly Dictionary<string, Value> _variables = new();
    private readonly int _depth;

    public VariablesTable(VariablesTable? parent = null)
    {
        _parent = parent;
        _depth = (parent?._depth ?? 0) + 1;
    }

    public VariablesTable? Parent => _parent;

    /// <summary>
    /// Получает таблицу с указанной глубиной.
    /// Это необходимо для поддержки захвата функцией окружающей её области видимости.
    /// </summary>
    public VariablesTable GetAncestor(int depth)
    {
        if (depth <= 0)
        {
            throw new InvalidOperationException($"Invalid variables table depth {depth}");
        }

        if (depth > _depth)
        {
            throw new InvalidOperationException(
                $"No variables table with depth {depth}: current depth is {_depth}"
            );
        }

        return GetAncestorImpl(depth);
    }

    /// <summary>
    /// Объявляет переменную с указанным именем и начальным значением.
    /// </summary>
    public void DefineVariable(string name, Value value)
    {
        if (!_variables.TryAdd(name, value))
        {
            throw new InvalidOperationException($"Variable {name} already defined");
        }
    }

    /// <summary>
    /// Присваивает значение переменной с указанным именем.
    /// Ищет переменную в текущей таблице и всех родительских.
    /// </summary>
    public void AssignVariable(string name, Value value)
    {
        if (_variables.ContainsKey(name))
        {
            _variables[name] = value;
            return;
        }

        if (_parent != null)
        {
            _parent.AssignVariable(name, value);
            return;
        }

        throw new InvalidOperationException($"Variable {name} not defined");
    }

    /// <summary>
    /// Получает значение переменной по имени.
    /// Ищет переменную в текущей таблице и всех родительских.
    /// </summary>
    public Value GetVariable(string name)
    {
        if (_variables.TryGetValue(name, out Value? value))
        {
            return value;
        }

        if (_parent != null)
        {
            return _parent.GetVariable(name);
        }

        throw new InvalidOperationException($"Variable {name} not found");
    }

    private VariablesTable GetAncestorImpl(int depth)
    {
        if (_depth == depth)
        {
            return this;
        }

        return _parent!.GetAncestor(depth);
    }
}