namespace VirtualMachine.Builtins;

/// <summary>
/// Код встроенной функции.
/// </summary>
public enum BuiltinFunctionCode
{
    /// <summary>
    /// `print(value)` — выводит значение в стандартный поток вывода
    /// </summary>
    Print = 1,

    /// <summary>
    /// readi() -> int — читает целое число из стандартного потока ввода
    /// </summary>
    ReadI = 2,

    /// <summary>
    /// readf() -> float — читает вещественное число из стандартного потока ввода
    /// </summary>
    ReadF = 3,

    /// <summary>
    /// reads() -> string — читает строку из стандартного потока ввода
    /// </summary>
    ReadS = 4,

    /// <summary>
    /// Length(s: string) -> int — возвращает длину строки
    /// </summary>
    Length = 5,

    /// <summary>
    /// Substr(s: string, start: int, length: int) -> string — возвращает подстроку
    /// </summary>
    Substr = 6,
}