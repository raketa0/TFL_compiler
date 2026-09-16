namespace Ast.Statements;

using Ast.Expressions;

using ValueType = Runtime.ValueType;

public sealed class VariableDeclarationStatement : AbstractVariableDeclarationStatemnt
{
    public VariableDeclarationStatement(string name, ValueType type, Expression? value)
        : base(name)
    {
        Type = type;
        Value = value;
    }

    public ValueType Type { get; }

    public Expression? Value { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}