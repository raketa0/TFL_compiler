# Список интеграционных тестов интерпретатора

## Точка входа (main)

- [x] Выполнение блока main
    - `main { print(100); }` → "100"
- [x] Выполнение main с несколькими выражениями
    - `main { var x: int = 10; var y: int = 20; print(x + y); print(x * y); }` → "30200"
- [x] Ошибка при отсутствии main
    - `{ print(10); }` → ошибка
- [x] Ошибка, если main не первый
    - `var x: int = 10; main { print(x); }` → ошибка
- [x] Пустое тело main
    - `main { }` → ""

## Встроенные функции (print/read)

- [x] Вывод чисел
    - `print(42); print(-10); print(0);` → "42-100"
- [x] Вывод выражений
    - `print(10 + 20); print(100 - 30); print(15 * 4); print(100 / 20);` → "3070605"
- [x] Чтение целого числа
    - `read(x); print(x);` → "42"
- [x] Чтение нескольких целых чисел
    - `read(a); read(b); print(a + b); print(a - b);` → "13070"
- [x] Чтение и использование в выражении
    - `read(x); read(y); print(x * y + x - y);` → "55"
- [x] Ошибка при чтении в константу
    - `const x: int = 10; read(x);` → ошибка
- [x] Вывод переменных
    - `print(a); print(b); print(a + b);` → "538"
- [x] Комбинация print и read
    - `print(1); read(x); print(x); print(2);` → "1992"
- [x] Ошибка при отсутствии входных данных
    - `read(x);` → ошибка
- [x] Чтение вещественного числа
    - `var x: float = 0.0; read(x); print(x);` → "3.5"
- [x] Чтение нескольких вещественных чисел
    - `read(a); read(b); print(a + b);` → "3.5"
- [x] Чтение строки
    - `var s: string = ""; read(s); print(s);` → "hello"
- [x] Чтение нескольких строк
    - `read(a); read(b); print(a + b);` → "hello world"

## Функции substr и length

- [x] substr возвращает середину строки
    - `print(substr("hello", 1, 3));` → "ell"
- [x] substr возвращает префикс
    - `print(substr("hello", 0, 3));` → "hel"
- [x] substr возвращает суффикс
    - `print(substr("hello", 2, 3));` → "llo"
- [x] substr возвращает пустую строку при длине 0
    - `print(substr("hello", 2, 0));` → ""
- [x] substr работает с переменной-строкой
    - `var s: string = "world"; print(substr(s, 1, 3));` → "orl"
- [x] substr работает с переменными-int для start и length
    - `var start: int = 2; var len: int = 3; print(substr(s, start, len));` → "cde"
- [x] substr совместно с length
    - `print(substr(s, 0, length(s) - 2));` → "hel"
- [x] результат substr можно присвоить переменной
    - `var sub: string = substr(s, 0, 7); print(sub);` → "program"
- [x] результат substr можно конкатенировать
    - `print(substr(s, 0, 5) + "!");` → "hello!"
- [x] substr корректно обрабатывает Unicode-символы 
    - `print(substr("Hello, 🚀", 7, 1));` → "🚀"
- [x] substr с Unicode-эмодзи использует позиции символов
    - `print(substr("🗿🗿🗿", 1, 2));` → "🗿🗿"
- [x] length возвращает длину строки
    - `print(length("hello"));` → "5"
- [x] length пустой строки равен 0
    - `print(length(""));` → "0"
- [x] length работает с переменной-строкой
    - `var s: string = "abc"; print(length(s));` → "3"
- [x] length считает символы Unicode, а не UTF-16 units
    - `print(length("🗿🗿🗿"));` → "3"

## Выражения (int)

- [x] Арифметика с приоритетом
    - `print(1 + 2 * 8 / 4 - 1);` → "4"
    - `print(10 + 20 * 2);` → "50"
    - `print(100 - 30 / 3);` → "90"
- [x] Арифметика со скобками
    - `print((1 + 2) * (8 / (3 - 1)));` → "12"
    - `print((10 - 5) * (2 + 3));` → "25"
- [x] Левоассоциативность
    - `print(10 - 3 - 2);` → "5"
    - `print(10 / 2 / 5);` → "1"
    - `print(10 - 3 + 2);` → "9"
    - `print(10 / 5 * 2);` → "4"
- [x] Унарный минус
    - `print(-4);` → "-4"
    - `print(2 * 2 * (-5));` → "-20"
    - `print(-10 + 5);` → "-5"
    - `print(10 + -5);` → "5"
- [x] Оператор остатка
    - `print(10 % 3);` → "1"
    - `print(17 % 5);` → "2"
    - `print(20 % 4);` → "0"
- [x] Сложные выражения
    - `print(2 + 3 * 4 - 10 / 2);` → "9"
    - `print((2 + 3) * (4 - 10) / 2);` → "-15"
- [x] Операторы сравнения
    - `print(1 != 2);` → "1"
    - `print(1 != 1);` → "0"
    - `print(1 == 1);` → "1"
    - `print(1 == 2);` → "0"
- [x] Операторы отношения
    - `print(1 < 2);` → "1"
    - `print(2 < 1);` → "0"
    - `print(1 < 1);` → "0"
    - `print(1 > 2);` → "0"
    - `print(2 > 1);` → "1"
    - `print(1 > 1);` → "0"
    - `print(1 <= 2);` → "1"
    - `print(1 <= 1);` → "1"
    - `print(2 <= 1);` → "0"
    - `print(1 >= 2);` → "0"
    - `print(1 >= 1);` → "1"
    - `print(2 >= 1);` → "1"

## Выражения (float)

- [x] Арифметика с приоритетом
    - `print(1.5 + 2.0);` → "3.5"
    - `print(10.0 - 3.5);` → "6.5"
    - `print(3.0 * 2.5);` → "7.5"
    - `print(10.0 / 4.0);` → "2.5"
- [x] Левоассоциативность
    - `print(10.0 - 3.0 - 2.5);` → "4.5"
    - `print(10.0 / 2.0 / 2.5);` → "2"
    - `print(10.0 - 3.0 + 2.5);` → "9.5"
    - `print(10.0 / 2.0 * 2.5);` → "12.5"
- [x] Унарный минус
    - `print(-1.5);` → "-1.5"
    - `print(2.0 * 2.0 * (-2.5));` → "-10"
    - `print(-10.0 + 5.0);` → "-5"
    - `print(10.0 + -5.0);` → "5"
- [x] Оператор остатка
    - `print(3.5 % 2.0);` → "1.5"
- [x] Сложные выражения
    - `print(1.5 + 2.0 * 3.0);` → "7.5"
    - `print((1.5 + 2.5) * 0.5);` → "2"
    - `print(2.5 + 3.0 * 4.0 - 10.0 / 2.0);` → "9.5"
    - `print((2.0 + 3.0) * (4.0 - 10.0) / 2.0);` → "-15"
- [x] Операторы сравнения
    - `print(1.5 == 1.5);` → "1"
    - `print(1.5 == 2.5);` → "0"
    - `print(1.5 != 2.5);` → "1"
    - `print(1.5 != 1.5);` → "0"
- [x] Операторы отношения
    - `print(1.0 < 2.0);` → "1"
    - `print(2.0 < 1.0);` → "0"
    - `print(1.5 <= 1.5);` → "1"
    - `print(2.5 > 1.5);` → "1"
    - `print(1.5 >= 2.5);` → "0"

## Выражения (string)

- [x] Конкатенация
    - `print("hello" + " world");` → "hello world"
    - `print("foo" + "bar");` → "foobar"
    - `print("a" + "b" + "c");` → "abc"
    - `print("x" + "y" + "z" + "w");` → "xyzw"
- [x] Конкатенация с пустой строкой
    - `print("hello" + "");` → "hello"
    - `print("" + "world");` → "world"
- [x] Операторы сравнения
    - `print("hello" == "hello");` → "1"
    - `print("hello" == "world");` → "0"
    - `print("hello" != "world");` → "1"
    - `print("hello" != "hello");` → "0"
    - `print("" == "");` → "1"
    - `print("" != "x");` → "1"
- [x] Операторы отношения
    - `print("abc" < "abd");` → "1"
    - `print("abc" > "abd");` → "0"
    - `print("abc" <= "abc");` → "1"
    - `print("xyz" >= "abc");` → "1"

## Логические выражения (bool)

- [x] Логическое НЕ
    - `print(!true);` → "0"
    - `print(!false);` → "1"
- [x] Логическое И
    - `print(true && true);` → "1"
    - `print(true && false);` → "0"
    - `print(false && true);` → "0"
    - `print(false && false);` → "0"
- [x] Логическое ИЛИ
    - `print(true || true);` → "1"
    - `print(true || false);` → "1"
    - `print(false || true);` → "1"
    - `print(false || false);` → "0"
- [x] Сравнение bool
    - `print(true == true);` → "1"
    - `print(true == false);` → "0"
    - `print(true != false);` → "1"
- [x] Короткая схема для &&
    - `var b: bool = false && (x == 1); print(b);` → "0"
- [x] Короткая схема для ||
    - `var b: bool = true || (x == 1); print(b);` → "1"

## Переменные и константы (int)

- [x] Объявление и использование переменных
    - `var x: int = 10; var y: int = 20; var z: int = x + y; print(z);` → "30"
- [x] Объявление без инициализации
    - `var x: int; x = 42; print(x);` → "42"
- [x] Значение по умолчанию равно 0
    - `var x: int; print(x);` → "0"
- [x] Переопределение переменной
    - `x = 25; print(x); x = x * 2; print(x);` → "2550"
- [x] Использование константы
    - `const x: int = 100; var y: int = x * 2; print(y);` → "200"
- [x] Ошибка при присваивании константе
    - `const x: int = 10; x = 20;` → ошибка
- [x] Ошибка при повторном объявлении
    - `var x: int = 10; var x: int = 20;` → ошибка
- [x] Использование переменных в выражении
    - `var c: int = (a + b) * (a - b); print(c);` → "16"
- [x] Вычисление площади прямоугольника
    - `width = x2 - x1; height = y2 - y1; square = width * height; print(square);` → "18"

## Переменные (float)

- [x] Объявление и использование
    - `var x: float = 1.5; var y: float = 2.0; var z: float = x + y; print(z);` → "3.5"
- [x] Переприсвоение
    - `var x: float = 1.5; x = 3.5; print(x); x = x * 2.0; print(x);` → "3.57"
- [x] Значение по умолчанию равно 0
    - `var x: float; print(x);` → "0"

## Переменные (string)

- [x] Объявление и использование
    - `var s: string = "hello"; print(s);` → "hello"
- [x] Конкатенация через переменные
    - `var a: string = "hello"; var b: string = " world"; var c: string = a + b; print(c);` → "hello world"
- [x] Значение по умолчанию — пустая строка
    - `var s: string; print(s);` → ""

## Переменные (bool)

- [x] Объявление и использование
    - `var b: bool = true; print(b);` → "1"
- [x] Значение по умолчанию — false
    - `var b: bool; print(b);` → "0"
- [x] Присваивание
    - `var b: bool = false; b = true; print(b);` → "1"
- [x] Константы bool
    - `const yes: bool = true; const no: bool = false; print(yes); print(no);` → "10"
- [x] false выводится как 0
    - `print(false);` → "0"
- [x] true выводится как 1
    - `print(true);` → "1"

## Управляющие конструкции (if/else)

- [x] if выполняет then-ветвь при true
    - `if (true) { print(1); }` → "1"
- [x] if пропускает тело при false
    - `if (false) { print(1); }` → ""
- [x] if-else выполняет then-ветвь
    - `if (x > 3) { print(1); } else { print(0); }` → "1"
- [x] if-else выполняет else-ветвь
    - `if (x > 3) { print(1); } else { print(0); }` → "0"
- [x] Цепочка else-if берет первую истинную ветвь
    - `if (x > 10) ... else if (x > 3) ... else ...` → "2"
- [x] Цепочка else-if берет else-ветвь
    - `if (x > 10) ... else if (x > 3) ... else ...` → "1"
- [x] if с выражением сравнения
    - `if (a < b) { print(1); }` → "1"
- [x] if с логическим И
    - `if (x > 3 && x < 10) { print(1); } else { print(0); }` → "1"
- [x] if с логическим ИЛИ
    - `if (x > 10 || x < 5) { print(1); } else { print(0); }` → "1"

## Управляющие конструкции (while)

- [x] while выполняет тело пока условие true
    - `while (i < 3) { print(i); i = i + 1; }` → "012"
- [x] while не выполняет тело при изначально ложном условии
    - `while (i < 3) { ... }` → ""
- [x] while с break выходит из цикла
    - `while (true) { if (i >= 3) { break; } print(i); i = i + 1; }` → "012"
- [x] while с continue пропускает остаток тела
    - `while (i < 5) { i = i + 1; if (i == 3) { continue; } print(i); }` → "1245"
- [x] Вложенные циклы while
    - `while (i < 2) { while (j < 2) { print(i); print(j); j = j + 1; } i = i + 1; }` → "00011011"
- [x] while с bool-переменной в условии
    - `while (running) { count = count + 1; if (count >= 3) { running = false; } } print(count);` → "3"

## Семантические ошибки (общие)

- [x] Использование необъявленной переменной
    - `print(x);` → ошибка
- [x] Использование необъявленной переменной в присваивании
    - `x = 10;` → ошибка
- [x] Присваивание константе
    - `const x: int = 10; x = 20;` → ошибка
- [x] Чтение в константу
    - `const x: int = 10; read(x);` → ошибка
- [x] Повторное объявление переменной
    - `var x: int = 10; var x: int = 20;` → ошибка
- [x] Повторное объявление константы
    - `const x: int = 10; const x: int = 20;` → ошибка

## Семантические ошибки (несовместимость типов при инициализации)

- [x] float в int-переменную
    - `var x: int = 1.5;` → ошибка
- [x] string в int-переменную
    - `var x: int = "hello";` → ошибка
- [x] int в float-переменную
    - `var x: float = 1;` → ошибка
- [x] string в float-переменную
    - `var x: float = "hello";` → ошибка
- [x] int в string-переменную
    - `var x: string = 1;` → ошибка
- [x] float в string-переменную
    - `var x: string = 1.5;` → ошибка

## Семантические ошибки (несовместимость типов при присваивании)

- [x] float в int-переменную
    - `var x: int = 0; x = 1.5;` → ошибка
- [x] int в float-переменную
    - `var x: float = 0.0; x = 1;` → ошибка
- [x] int в string-переменную
    - `var x: string = ""; x = 1;` → ошибка

## Семантические ошибки (несовместимые типы в операциях)

- [x] int + float
    - `print(1 + 1.5);` → ошибка
- [x] string - string
    - `print("a" - "b");` → ошибка
- [x] string * int
    - `print("hello" * 2);` → ошибка
- [x] float + string
    - `print(1.5 + "x");` → ошибка
- [x] string / int
    - `print("hello" / 2);` → ошибка
- [x] bool + bool
    - `print(true + false);` → ошибка
- [x] int < float
    - `print(1 < 1.5);` → ошибка
- [x] bool == int
    - `print(true == 1);` → ошибка
- [x] string && string
    - `print("hello" && "world");` → ошибка
- [x] float || float
    - `print(1.0 || 2.0);` → ошибка
- [x] Унарный минус к строке
    - `print(-"hello");` → ошибка
- [x] Унарный минус к bool
    - `print(-true);` → ошибка
- [x] ! к int
    - `print(!x);` где x: int → ошибка
- [x] ! к float
    - `print(!x);` где x: float → ошибка
- [x] ! к string
    - `print(!x);` где x: string → ошибка
- [x] && с int
    - `print(1 && 0);` → ошибка
- [x] || с int
    - `print(0 || 1);` → ошибка
- [x] < с bool
    - `print(true < false);` → ошибка
- [x] length от int
    - `print(length(42));` → ошибка
- [x] length от float
    - `print(length(1.5));` → ошибка

## Семантические ошибки (substr)

- [x] Первый аргумент не строка
    - `print(substr(42, 0, 1));` → ошибка
- [x] Второй аргумент не int
    - `print(substr("hello", "a", 1));` → ошибка
- [x] Третий аргумент не int
    - `print(substr("hello", 0, "bad"));` → ошибка

## Семантические ошибки (управляющие конструкции)

- [x] Условие if не bool
    - `if (x) { print(1); }` где x: int → ошибка
- [x] Условие while не bool
    - `while (x) { ... }` где x: int → ошибка
- [x] break вне while
    - `break;` → ошибка
- [x] continue вне while
    - `continue;` → ошибка
- [x] break в if вне while
    - `if (true) { break; }` → ошибка
- [x] continue в if вне while
    - `if (true) { continue; }` → ошибка

## Семантические ошибки (read)

- [x] Чтение в bool-переменную
    - `var b: bool; read(b);` → ошибка

## Пользовательские функции

- [ ] Функция возвращает int: `func double(n: int): int { return n + n; }` → double(5) → "10"
- [ ] Void-функция выполняет побочный эффект: `func greet(): void { print("hello"); }` → greet() → "hello"
- [ ] Функция с двумя параметрами: `func add(a: int, b: int): int { return a + b; }` → add(3, 4) → "7"
- [ ] Функция вызывается несколько раз: inc(1), inc(2), inc(3) → "234"
- [ ] Вложенные вызовы: outer вызывает inner → "15"
- [ ] Функция с локальными переменными: compute(5) → "11"
- [ ] Функция с if/else: max(3, 7) → "7", max(10, 4) → "10"
- [ ] Результат функции используется в выражении: double(3) + 1 → "7"
- [ ] Несколько функций: square(3) → "9", cube(2) → "8"
- [ ] Переменные вызывающего не меняются после вызова: x=10 → "10", result=6
- [ ] Функция с float-параметром: halve(10.0) → "5"
- [ ] Функция с string-параметром: greet("world") → "hello world"
- [ ] Функция с циклом while: sumTo(4) → "10"