using Ast.Expressions;

namespace Ast.Statements;

public class WhileStatement : Statement
{
    public WhileStatement(Expression expression, BlockStatement block)
    {
        Expression = expression;
        Block = block;
    }

    public Expression Expression { get; }

    public BlockStatement Block { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}