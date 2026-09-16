namespace Ast.Expressions;

public sealed class LengthExpression : Expression
{
    public LengthExpression(Expression operand)
    {
        Operand = operand;
    }

    public Expression Operand { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}