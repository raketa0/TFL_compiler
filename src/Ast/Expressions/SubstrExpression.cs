namespace Ast.Expressions;

public sealed class SubstrExpression : Expression
{
    public SubstrExpression(Expression source, Expression start, Expression length)
    {
        Source = source;
        Start = start;
        Length = length;
    }

    public Expression Source { get; }

    public Expression Start { get; }

    public Expression Length { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}