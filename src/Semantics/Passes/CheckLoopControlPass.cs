using Ast.Statements;

using Semantics.Exceptions;

namespace Semantics.Passes;

public class CheckLoopControlPass : AbstractPass
{
    private int _loopDepth = 0;

    public override void Visit(WhileStatement s)
    {
        _loopDepth++;

        try
        {
            base.Visit(s);
        }
        finally
        {
            _loopDepth--;
        }
    }

    public override void Visit(BreakStatement s)
    {
        if (_loopDepth == 0)
        {
            throw new InvalidLoopControlStatementException("break");
        }

        base.Visit(s);
    }

    public override void Visit(ContinueStatement s)
    {
        if (_loopDepth == 0)
        {
            throw new InvalidLoopControlStatementException("continue");
        }

        base.Visit(s);
    }
}