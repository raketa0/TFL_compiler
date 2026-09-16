using Ast.Attributes;
using Ast.Statements;

namespace Ast.Expressions;

public sealed class VariableExpression : Expression
{
    private AstAttribute<AbstractVariableDeclarationStatemnt> _variable;

    public VariableExpression(string name)
    {
        Name = name;
    }

    public string Name { get; }

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