using Ast.Program;

using Semantics.Passes;
using Semantics.Symbols;

namespace Semantics;

/// <summary>
/// Класс для проверки семантики программы.
/// Реализован как фасад над несколькими проходами (passes), каждый из которых реализует шаблон «Посетитель» (Visitor).
/// </summary>
public class SemanticsChecker
{
    private readonly AbstractPass[] _passes;

    public SemanticsChecker()
    {
        SymbolsTable globalSymbols = new SymbolsTable(parent: null);

        _passes =
        [
            new ResolveNamePass(globalSymbols),
            new ResolveTypesPass(),
            new CheckTypesPass(),
            new CheckLoopControlPass(),
            new CheckReturnPathPass(),
        ];
    }

    public void Check(ProgramNode program)
    {
        foreach (AbstractPass pass in _passes)
        {
            program.Accept(pass);
        }
    }
}