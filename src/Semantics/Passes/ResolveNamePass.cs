using Ast;
using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

using Semantics.Exceptions;
using Semantics.Symbols;

namespace Semantics.Passes;

public sealed class ResolveNamePass : AbstractPass
{
    private SymbolsTable _symbols;

    public ResolveNamePass(SymbolsTable globalSymbols)
    {
        _symbols = globalSymbols;
    }

    public override void Visit(ProgramNode p)
    {
        // Регистрируем все функции до обхода тел — это позволяет вызывать функции
        // до их объявления и поддерживает взаимную рекурсию.
        foreach (FunctionDeclarationStatement f in p.Functions)
        {
            _symbols.DeclareFunction(f);
        }

        base.Visit(p);
    }

    public override void Visit(FunctionDeclarationStatement s)
    {
        _symbols = new SymbolsTable(_symbols);
        try
        {
            base.Visit(s);
        }
        finally
        {
            _symbols = _symbols.Parent!;
        }
    }

    public override void Visit(ParametrDeclaration d)
    {
        base.Visit(d);
        _symbols.DeclareVariable(d);
    }

    public override void Visit(VariableDeclarationStatement s)
    {
        _symbols.DeclareVariable(s);
        base.Visit(s);
    }

    public override void Visit(ConstDeclarationStatement s)
    {
        _symbols.DeclareVariable(s);
        base.Visit(s);
    }

    public override void Visit(AssignmentStatement s)
    {
        Statement decl = _symbols.GetVariableDeclaration(s.Name);

        if (decl is ConstDeclarationStatement)
        {
            throw new InvalidAssignmentException($"Cannot assign to constant '{s.Name}'");
        }

        s.Variable = (AbstractVariableDeclarationStatemnt)decl;

        base.Visit(s);
    }

    public override void Visit(ReadStatement s)
    {
        Statement decl = _symbols.GetVariableDeclaration(s.Name);

        if (decl is ConstDeclarationStatement)
        {
            throw new InvalidAssignmentException("Cannot read into const variable");
        }

        s.Variable = (AbstractVariableDeclarationStatemnt)decl;
    }

    public override void Visit(VariableExpression e)
    {
        base.Visit(e);
        e.Variable = _symbols.GetVariableDeclaration(e.Name);
    }

    public override void Visit(FunctionCallExpression e)
    {
        base.Visit(e);
        e.Function = _symbols.GetFunctionDeclaration(e.Name);
    }
}