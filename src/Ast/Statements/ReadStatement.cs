namespace Ast.Statements;

public sealed class ReadStatement : Statement
{
    public ReadStatement(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public AbstractVariableDeclarationStatemnt? Variable { get; set; }

    public new Runtime.ValueType? ResultType { get; set; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}