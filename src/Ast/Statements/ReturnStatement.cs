using Ast.Expressions;

namespace Ast.Statements;

public sealed class ReturnStatement : Statement
{
    public ReturnStatement(Expression? expression)
    {
        Expression = expression;
    }

    public Expression? Expression { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}