using Ast.Attributes;
using Ast.Statements;

namespace Ast.Expressions;

public class FunctionCallExpression : Expression
{
    private AstAttribute<AbstractFunctionDeclarationStatement> _function;

    public FunctionCallExpression(string name, IReadOnlyList<Expression> arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public string Name { get; }

    public AbstractFunctionDeclarationStatement Function
    {
        get => _function.Get();
        set => _function.Set(value);
    }

    public IReadOnlyList<Expression> Arguments { get; }

    public override void Accept(IAstVisitor visitor)
    {
        visitor.Visit(this);
    }
}