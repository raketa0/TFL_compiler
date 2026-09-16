namespace Lexemes;

public enum TokenType
{
    /// <summary>
    /// Ключевое слово main
    /// </summary>
    Main,

    /// <summary>
    /// Ключевое слово var
    /// </summary>
    Var,

    /// <summary>
    /// Ключевое слово const
    /// </summary>
    Const,

    /// <summary>
    /// Ключевое слово read
    /// </summary>
    Read,

    /// <summary>
    /// Ключевое слово print
    /// </summary>
    Print,

    /// <summary>
    /// Ключевое слово length
    /// </summary>
    Length,

    /// <summary>
    /// Ключевое слово substr
    /// </summary>
    Substr,

    /// <summary>
    /// Ключевое слово if
    /// </summary>
    If,

    /// <summary>
    /// Ключевое слово else
    /// </summary>
    Else,

    /// <summary>
    /// Ключевое слово break
    /// </summary>
    Break,

    /// <summary>
    /// Ключевое слово continue
    /// </summary>
    Continue,

    /// <summary>
    /// Ключевое слово return
    /// </summary>
    Return,

    /// <summary>
    /// Ключевое слово while
    /// </summary>
    While,

    /// <summary>
    /// Ключевое слово func
    /// </summary>
    Func,

    /// <summary>
    /// Ключевое слово void
    /// </summary>
    Void,

    /// <summary>
    /// Идентификатор
    /// </summary>
    Identifier,

    /// <summary>
    /// Литерал целого числа
    /// </summary>
    IntLiteral,

    /// <summary>
    /// Литерал вещественного числа
    /// </summary>
    FloatLiteral,

    /// <summary>
    /// Литерал строки
    /// </summary>
    StringLiteral,

    /// <summary>
    /// Литерал bool
    /// </summary>
    BoolLiteral,

    /// <summary>
    /// Целочисленный тип
    /// </summary>
    IntegerType,

    /// <summary>
    /// Вечественный тип
    /// </summary>
    FloatType,

    /// <summary>
    /// Строковый тип
    /// </summary>
    StringType,

    /// <summary>
    /// Булевый тип
    /// </summary>
    BooleanType,

    /// <summary>
    /// Запятая
    /// </summary>
    Comma,

    /// <summary>
    /// Точка с запятой
    /// </summary>
    Semicolon,

    /// <summary>
    /// Двоеточие
    /// </summary>
    Colon,

    /// <summary>
    /// Открываюшая круглая скобка
    /// </summary>
    OpenParenthesis,

    /// <summary>
    /// Закрывающая круглая скобка
    /// </summary>
    CloseParenthesis,

    /// <summary>
    /// Открвающая фигурная скобка
    /// </summary>
    OpenBrace,

    /// <summary>
    /// Закрывающая фигурная скобка
    /// </summary>
    CloseBrace,

    /// <summary>
    /// Оператор присваивания =
    /// </summary>
    Assign,

    /// <summary>
    /// Оператор сложения/конкатенации +
    /// </summary>
    Plus,

    /// <summary>
    /// Оператор вычитания -
    /// </summary>
    Minus,

    /// <summary>
    /// Оператор умножения *
    /// </summary>
    Multiply,

    /// <summary>
    /// Оператор деления /
    /// </summary>
    Divide,

    /// <summary>
    /// Оператор остатка от деления %
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
    /// Оператор равенства ==
    /// </summary>
    Equal,

    /// <summary>
    /// Оператор неравенства !=
    /// </summary>
    NotEqual,

    /// <summary>
    /// Логический оператор и &&
    /// </summary>
    LogicalAnd,

    /// <summary>
    /// Логический оператор или ||
    /// </summary>
    LogicalOr,

    /// <summary>
    /// Логическое не !
    /// </summary>
    LogicalNot,

    /// <summary>
    /// Конец потока токенов
    /// </summary>
    EndOfFile,

    /// <summary>
    /// Недопустимая лексемма
    /// </summary>
    Error,
}