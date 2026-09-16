using Ast;
using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

namespace Semantics.Passes;

public abstract class AbstractPass : IAstVisitor
{
    public virtual void Visit(ProgramNode p)
    {
        foreach (FunctionDeclarationStatement f in p.Functions)
        {
            f.Accept(this);
        }

        p.Block.Accept(this);
    }

    public virtual void Visit(LiteralExpression e)
    {
    }

    public virtual void Visit(FunctionCallStatement s)
    {
        s.Expression.Accept(this);
    }

    public virtual void Visit(BlockStatement s)
    {
        foreach (Statement statement in s.Statements)
        {
            statement.Accept(this);
        }
    }

    public virtual void Visit(AssignmentStatement s)
    {
        s.Expression.Accept(this);
    }

    public virtual void Visit(VariableDeclarationStatement s)
    {
        s.Value?.Accept(this);
    }

    public virtual void Visit(ConstDeclarationStatement s)
    {
        s.Value.Accept(this);
    }

    public virtual void Visit(ReadStatement s)
    {
    }

    public virtual void Visit(PrintStatement s)
    {
        s.Expression.Accept(this);
    }

    public virtual void Visit(IfElseStatement s)
    {
        s.Condition.Accept(this);
        s.Block.Accept(this);
        s.ElseStatement?.Accept(this);
    }

    public virtual void Visit(ContinueStatement s)
    {
    }

    public virtual void Visit(BreakStatement s)
    {
    }

    public virtual void Visit(WhileStatement s)
    {
        s.Expression.Accept(this);
        s.Block.Accept(this);
    }

    public virtual void Visit(ReturnStatement s)
    {
        s.Expression?.Accept(this);
    }

    public virtual void Visit(FunctionDeclarationStatement s)
    {
        foreach (AbstractParametrStatement param in s.Parameters)
        {
            param.Accept(this);
        }

        s.Body.Accept(this);
    }

    public virtual void Visit(ParametrDeclaration s)
    {
    }

    public virtual void Visit(BinaryOperationExpression e)
    {
        e.Left.Accept(this);
        e.Right.Accept(this);
    }

    public virtual void Visit(UnaryOperationExpression e)
    {
        e.Operand.Accept(this);
    }

    public virtual void Visit(VariableExpression e)
    {
    }

    public virtual void Visit(LengthExpression e)
    {
        e.Operand.Accept(this);
    }

    public virtual void Visit(FunctionCallExpression e)
    {
        foreach (Expression arg in e.Arguments)
        {
            arg.Accept(this);
        }
    }

    public virtual void Visit(SubstrExpression e)
    {
        e.Source.Accept(this);
        e.Start.Accept(this);
        e.Length.Accept(this);
    }
}