namespace Ast.Expressions;

public enum BinaryOperation
{
    /// <summary>
    /// Сложение чисел.
    /// </summary>
    Add,

    /// <summary>
    /// Вычитание чисел.
    /// </summary>
    Subtract,

    /// <summary>
    /// Умножение чисел.
    /// </summary>
    Multiply,

    /// <summary>
    /// Деление чисел.
    /// </summary>
    Divide,

    /// <summary>
    /// Деление чисел с остатком.
    /// </summary>
    Module,

    /// <summary>
    /// Оператор меньше
    /// </summary>
    LessThan,

    /// <summary>
    /// Оператор меньше или равно
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Оператор больше
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Оператор больше или равно
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Оператор равенства.
    /// </summary>
    Equal,

    /// <summary>
    /// Логическое и
    /// </summary>
    LogicalAnd,

    /// <summary>
    /// Логическое или
    /// </summary>
    LogicalOr,

    /// <summary>
    /// Оператор неравенства.
    /// </summary>
    NotEqual,
}