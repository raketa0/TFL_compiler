using Ast.Expressions;

namespace Ast.Statements;

public class FunctionCallStatement : Statement
{
    public FunctionCallStatement(FunctionCallExpression expression)
    {
        Expression = expression;
    }

    public FunctionCallExpression Expression { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}