using Ast.Attributes;

using ValueType = Runtime.ValueType;

namespace Ast.Statements;

public abstract class Statement : AstNode
{
    private AstAttribute<ValueType> _resultType;

    public ValueType ResultType
    {
        get => _resultType.Get();

        set => _resultType.Set(value);
    }
}