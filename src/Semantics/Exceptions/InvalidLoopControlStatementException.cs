namespace Semantics.Exceptions;

#pragma warning disable RCS1194 // Конструкторы исключения не нужны, т.к. это не класс общего назначения.

public sealed class InvalidLoopControlStatementException : Exception
{
    public InvalidLoopControlStatementException(string message)
        : base($"{message} statement is allowed only inside while loop")
    {
    }
}
#pragma warning restore RCS1194