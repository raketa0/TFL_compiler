using System.Diagnostics;

using Ast.Statements;

using Semantics.Exceptions;

namespace Semantics.Symbols;

public sealed class SymbolsTable
{
    private readonly SymbolsTable? _parent;
    private readonly Dictionary<string, Statement> _variables = [];
    private readonly Dictionary<string, AbstractFunctionDeclarationStatement> _functions = [];

    public SymbolsTable(SymbolsTable? parent)
    {
        _parent = parent;
    }

    public SymbolsTable? Parent => _parent;

    public AbstractVariableDeclarationStatemnt GetVariableDeclaration(string name)
    {
        if (_variables.TryGetValue(name, out Statement? declaration))
        {
            return declaration switch
            {
                AbstractVariableDeclarationStatemnt variable => variable,
                _ => throw new UnreachableException(),
            };
        }

        if (_parent != null)
        {
            return _parent.GetVariableDeclaration(name);
        }

        throw UnknownSymbolException.UndefinedVariableOrFunction(name);
    }

    public AbstractFunctionDeclarationStatement GetFunctionDeclaration(string name)
    {
        if (_functions.TryGetValue(name, out AbstractFunctionDeclarationStatement? declaration))
        {
            return declaration;
        }

        if (_parent != null)
        {
            return _parent.GetFunctionDeclaration(name);
        }

        throw UnknownSymbolException.UndefinedVariableOrFunction(name);
    }

    public void DeclareVariable(Statement symbol)
    {
        string name = symbol switch
        {
            VariableDeclarationStatement v => v.Name,
            ConstDeclarationStatement c => c.Name,
            AbstractParametrStatement p => p.Name,
            _ => throw new UnreachableException(),
        };

        if (!_variables.TryAdd(name, symbol))
        {
            throw DuplicateSymbolException.DuplicateVariableOrFunction(name);
        }
    }

    public void DeclareFunction(AbstractFunctionDeclarationStatement symbol)
    {
        if (!_functions.TryAdd(symbol.Name, symbol))
        {
            throw DuplicateSymbolException.DuplicateVariableOrFunction(symbol.Name);
        }
    }
}