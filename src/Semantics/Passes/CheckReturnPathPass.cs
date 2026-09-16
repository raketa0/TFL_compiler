using Ast.Program;
using Ast.Statements;

using Semantics.Exceptions;

using ValueType = Runtime.ValueType;

namespace Semantics.Passes;

/// <summary>
/// Проход по AST для проверки путей возврата из функций.
/// Проверяет:
/// — все пути в non-void функции гарантированно завершаются return;
/// — нет недостижимого кода после гарантированного return.
/// </summary>
public sealed class CheckReturnPathPass : AbstractPass
{
    public override void Visit(FunctionDeclarationStatement s)
    {
        base.Visit(s);

        if (s.ReturnType != ValueType.Void && !BlockGuaranteedReturn(s.Body))
        {
            throw new MissingReturnException(s.Name);
        }
    }

    public override void Visit(BlockStatement s)
    {
        CheckUnreachableInBlock(s);
        base.Visit(s);
    }

    private static bool BlockGuaranteedReturn(BlockStatement block)
    {
        foreach (Statement stmt in block.Statements)
        {
            if (StatementGuaranteedReturn(stmt))
            {
                return true;
            }
        }

        return false;
    }

    private static bool StatementGuaranteedReturn(Statement stmt) =>
        stmt switch
        {
            ReturnStatement => true,
            IfElseStatement ifElse => IfElseGuaranteedReturn(ifElse),
            BlockStatement block => BlockGuaranteedReturn(block),
            _ => false,
        };

    private static bool IfElseGuaranteedReturn(IfElseStatement s)
    {
        if (s.ElseStatement == null)
        {
            return false;
        }

        return StatementGuaranteedReturn(s.Block) && StatementGuaranteedReturn(s.ElseStatement);
    }

    private static void CheckUnreachableInBlock(BlockStatement block)
    {
        bool seenReturn = false;
        foreach (Statement stmt in block.Statements)
        {
            if (seenReturn)
            {
                throw new UnreachableCodeException();
            }

            if (StatementGuaranteedReturn(stmt))
            {
                seenReturn = true;
            }
        }
    }
}