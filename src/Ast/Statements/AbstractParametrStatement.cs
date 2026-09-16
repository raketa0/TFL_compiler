namespace Ast.Statements;

public abstract class AbstractParametrStatement : AbstractVariableDeclarationStatemnt
{
    protected AbstractParametrStatement(string name)
        : base(name)
    {
    }
}