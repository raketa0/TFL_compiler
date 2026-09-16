namespace Ast.Statements;

using ValueType = Runtime.ValueType;

public class ParametrDeclaration : AbstractParametrStatement
{
    public ParametrDeclaration(string name, ValueType type)
        : base(name)
    {
        Type = type;
    }

    public ValueType Type { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}