using Ast.Expressions;

namespace Ast.Statements;

public class IfElseStatement : Statement
{
    public IfElseStatement(Expression condition, BlockStatement block, Statement? elseStatement)
    {
        Condition = condition;
        Block = block;
        ElseStatement = elseStatement;
    }

    public Expression Condition { get; }

    public BlockStatement Block { get; }

    public Statement? ElseStatement { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}