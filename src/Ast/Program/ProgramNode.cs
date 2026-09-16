using Ast.Statements;

namespace Ast.Program;

public class ProgramNode : AstNode
{
    public ProgramNode(IReadOnlyList<FunctionDeclarationStatement> functions, BlockStatement block)
    {
        Functions = functions;
        Block = block;
    }

    public IReadOnlyList<FunctionDeclarationStatement> Functions { get; }

    public BlockStatement Block { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}