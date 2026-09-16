namespace Ast.Statements;

public abstract class AbstractVariableDeclarationStatemnt : Statement
{
    protected AbstractVariableDeclarationStatemnt(string name)
    {
        Name = name;
    }

    public string Name { get; }
}