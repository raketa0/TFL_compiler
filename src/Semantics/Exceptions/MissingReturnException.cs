namespace Semantics.Exceptions;

#pragma warning disable RCS1194

public sealed class MissingReturnException : Exception
{
    public MissingReturnException(string functionName)
        : base($"Non-void function '{functionName}' does not return a value on all code paths")
    {
    }
}

#pragma warning restore RCS1194