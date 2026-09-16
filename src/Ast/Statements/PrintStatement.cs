using Ast.Expressions;

namespace Ast.Statements;

public sealed class PrintStatement : Statement
{
    public PrintStatement(Expression expression)
    {
         Expression = expression;
    }

    public Expression Expression { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}