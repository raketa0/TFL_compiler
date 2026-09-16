using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

namespace Ast;

public interface IAstVisitor
{
    void Visit(ProgramNode p);

    void Visit(BinaryOperationExpression e);

    void Visit(LiteralExpression e);

    void Visit(UnaryOperationExpression e);

    void Visit(VariableExpression e);

    void Visit(LengthExpression e);

    void Visit(FunctionCallExpression e);

    void Visit(SubstrExpression e);

    void Visit(FunctionCallStatement s);

    void Visit(BlockStatement s);

    void Visit(AssignmentStatement s);

    void Visit(VariableDeclarationStatement s);

    void Visit(ConstDeclarationStatement s);

    void Visit(ReadStatement s);

    void Visit(PrintStatement s);

    void Visit(IfElseStatement s);

    void Visit(ContinueStatement s);

    void Visit(BreakStatement s);

    void Visit(WhileStatement s);

    void Visit(ReturnStatement s);

    void Visit(FunctionDeclarationStatement s);

    void Visit(ParametrDeclaration s);
}