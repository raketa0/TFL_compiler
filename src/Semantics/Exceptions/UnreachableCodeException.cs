namespace Semantics.Exceptions;

#pragma warning disable RCS1194

public sealed class UnreachableCodeException : Exception
{
    public UnreachableCodeException()
        : base("Unreachable code detected after return statement")
    {
    }
}

#pragma warning restore RCS1194