namespace Ast.Statements;

public abstract class AbstractFunctionDeclarationStatement : Statement
{
    protected AbstractFunctionDeclarationStatement(
        string name,
        IReadOnlyList<AbstractParametrStatement> parameters)
    {
        Name = name;
        Parameters = parameters;
    }

    public string Name { get; }

    public IReadOnlyList<AbstractParametrStatement> Parameters { get; }
}