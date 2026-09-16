# Синтаксис языка Swiston

## 1. Общая структура программы

| Свойство                 | Требование                  |
| ------------------------ | --------------------------- |
| Файл                     | Один                        |
| Точка входа              | Ровно один блок `main`      |
| Пользовательские функции | Поддерживаются (Эпик 2)     |

В Эпике 2 к программе можно добавить ноль или более объявлений
пользовательских функций. Все объявления функций предшествуют блоку `main`.

------------------------------------------------------------------------

## 2. Приоритет и ассоциативность операторов

От высшего к низшему:

| Приоритет | Операторы              | Ассоциативность |
| --------- | ---------------------- | --------------- |
| 1         | `-` (унарный), `!`     | Правая          |
| 2         | `*`, `/`, `%`          | Левая           |
| 3         | `+`, `-`               | Левая           |
| 4         | `<`, `>`, `<=`, `>=`   | Левая           |
| 5         | `==`, `!=`             | Левая           |
| 6         | `&&`                   | Левая           |
| 7         | `\|\|`                  | Левая           |

------------------------------------------------------------------------

## 3. Полная ISO EBNF-грамматика

``` ebnf
program =
    { func_decl },
    "main",
    block ;

block =
    "{",
    { statement },
    "}" ;

statement =
      var_decl
    | const_decl
    | assignment
    | print_stmt
    | read_stmt
    | if_stmt
    | while_stmt
    | break_stmt
    | continue_stmt
    | return_stmt
    | call_stmt
    | block ;

var_decl =
    "var",
    identifier,
    ":",
    type,
    [ "=", expression ],
    ";" ;

const_decl =
    "const",
    identifier,
    ":",
    type,
    "=",
    expression,
    ";" ;

assignment =
    identifier,
    "=",
    expression,
    ";" ;

print_stmt =
    "print",
    "(",
    expression,
    ")",
    ";" ;

read_stmt =
    "read",
    "(",
    identifier,
    ")",
    ";" ;

if_stmt =
    "if",
    "(",
    expression,
    ")",
    block,
    [ "else", ( block | if_stmt ) ] ;

while_stmt =
    "while",
    "(",
    expression,
    ")",
    block ;

break_stmt =
    "break",
    ";" ;

continue_stmt =
    "continue",
    ";" ;

return_stmt =
    "return",
    [ expression ],
    ";" ;

call_stmt =
    identifier,
    "(",
    [ expression, { ",", expression } ],
    ")",
    ";" ;

func_decl =
    "func",
    identifier,
    "(",
    [ param_decl, { ",", param_decl } ],
    ")",
    ":",
    return_type,
    block ;

param_decl =
    identifier,
    ":",
    type ;

return_type =
      type
    | "void" ;

type =
      "int"
    | "float"
    | "string"
    | "bool" ;

expression = logical_or ;

logical_or =
    logical_and,
    { "||", logical_and } ;

logical_and =
    equality,
    { "&&", equality } ;

equality =
    relational,
    { ( "==" | "!=" ), relational } ;

relational =
    additive,
    { ( "<" | ">" | "<=" | ">=" ), additive } ;

additive =
    multiplicative,
    { ( "+" | "-" ), multiplicative } ;

multiplicative =
    unary,
    { ( "*" | "/" | "%" ), unary } ;

unary =
      "-", unary
    | "!", unary
    | primary ;

primary =
      literal
    | identifier
    | call_expression
    | builtin_call
    | "(",
      expression,
      ")" ;

call_expression =
    identifier,
    "(",
    [ expression, { ",", expression } ],
    ")" ;

builtin_call =
      length_call
    | substr_call ;

length_call =
    "length",
    "(",
    expression,
    ")" ;

substr_call =
    "substr",
    "(",
    expression,
    ",",
    expression,
    ",",
    expression,
    ")" ;

literal =
      int_literal
    | float_literal
    | string_literal
    | bool_literal ;

bool_literal = "true" | "false" ;
```

> Терминальные символы `identifier`, `int_literal`, `float_literal`, `string_literal` определены в [`01_lexemes.md`](01_lexemes.md).