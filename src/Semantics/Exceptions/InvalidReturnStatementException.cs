namespace Semantics.Exceptions;

#pragma warning disable RCS1194

public sealed class InvalidReturnStatementException : Exception
{
    public InvalidReturnStatementException(string message)
        : base(message)
    {
    }
}

#pragma warning restore RCS1194