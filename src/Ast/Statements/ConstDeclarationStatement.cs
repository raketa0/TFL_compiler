namespace Ast.Statements;

using Ast.Expressions;

using ValueType = Runtime.ValueType;

public sealed class ConstDeclarationStatement : AbstractVariableDeclarationStatemnt
{
    public ConstDeclarationStatement(string name, ValueType type, Expression value)
        : base(name)
    {
        Type = type;
        Value = value;
    }

    public ValueType Type { get; }

    public Expression Value { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}