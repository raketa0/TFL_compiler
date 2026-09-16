using Ast.Attributes;
using Ast.Expressions;

namespace Ast.Statements;

public sealed class AssignmentStatement : Statement
{
    private AstAttribute<AbstractVariableDeclarationStatemnt> _variable;

    public AssignmentStatement(string name, Expression expression)
    {
        Name = name;
        Expression = expression;
    }

    public string Name { get; }

    public Expression Expression { get; }

    public AbstractVariableDeclarationStatemnt Variable
    {
        get => _variable.Get();
        set => _variable.Set(value);
    }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}