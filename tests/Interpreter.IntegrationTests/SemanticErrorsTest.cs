using Interpreter;

using Runtime;

using Semantics.Exceptions;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class SemanticErrorsTest
{
    [Fact]
    public void Throws_on_undefined_variable()
    {
        const string code = """
            main {
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<UnknownSymbolException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_undefined_variable_in_assignment()
    {
        const string code = """
            main {
                x = 10;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<UnknownSymbolException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_assign_to_const()
    {
        const string code = """
            main {
                const x: int = 10;
                x = 20;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidAssignmentException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_read_to_const()
    {
        const string code = """
            main {
                const x: int = 10;
                read(x);
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidAssignmentException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_duplicate_variable_declaration()
    {
        const string code = """
            main {
                var x: int = 10;
                var x: int = 20;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<DuplicateSymbolException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_duplicate_const_declaration()
    {
        const string code = """
            main {
                const x: int = 10;
                const x: int = 20;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<DuplicateSymbolException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_float_assigned_to_int_variable()
    {
        const string code = """
            main {
                var x: int = 1.5;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_string_assigned_to_int_variable()
    {
        const string code = """
            main {
                var x: int = "hello";
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_assigned_to_float_variable()
    {
        const string code = """
            main {
                var x: float = 1;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_string_assigned_to_float_variable()
    {
        const string code = """
            main {
                var x: float = "hello";
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_assigned_to_string_variable()
    {
        const string code = """
            main {
                var x: string = 1;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_float_assigned_to_string_variable()
    {
        const string code = """
            main {
                var x: string = 1.5;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_float_reassigned_to_int_variable()
    {
        const string code = """
            main {
                var x: int = 0;
                x = 1.5;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_reassigned_to_float_variable()
    {
        const string code = """
            main {
                var x: float = 0.0;
                x = 1;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_reassigned_to_string_variable()
    {
        const string code = """
            main {
                var x: string = "";
                x = 1;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_plus_float()
    {
        const string code = """
            main {
                print(1 + 1.5);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_string_minus_string()
    {
        const string code = """
            main {
                print("a" - "b");
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_string_multiply_int()
    {
        const string code = """
            main {
                print("hello" * 2);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_float_plus_string()
    {
        const string code = """
            main {
                print(1.5 + "x");
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_unary_minus_on_string()
    {
        const string code = """
            main {
                print(-"hello");
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_unary_minus_on_bool()
    {
        const string code = """
                            main {
                                print(-true);
                            }
                            """;
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_length_of_int()
    {
        const string code = """
            main {
                print(length(42));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_length_of_float()
    {
        const string code = """
            main {
                print(length(1.5));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Substr_throws_on_non_string_source()
    {
        const string code = """
            main {
                print(substr(42, 0, 1));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Substr_throws_on_non_int_start()
    {
        const string code = """
            main {
                print(substr("hello", "a", 1));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Substr_throws_on_non_int_length()
    {
        const string code = """
            main {
                print(substr("hello", 0, "bad"));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_logical_not_applied_to_int()
    {
        const string code = """
            main {
                var x: int = 1;
                print(!x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_logical_not_applied_to_float()
    {
        const string code = """
            main {
                var x: float = 1.0;
                print(!x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_logical_not_applied_to_string()
    {
        const string code = """
            main {
                var x: string = "";
                print(!x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_logical_and_with_int_operands()
    {
        const string code = """
            main {
                print(1 && 0);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_logical_or_with_int_operands()
    {
        const string code = """
            main {
                print(0 || 1);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_less_than_with_bool_operands()
    {
        const string code = """
            main {
                print(true < false);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_if_condition_not_bool()
    {
        const string code = """
            main {
                var x: int = 1;
                if (x) { print(1); }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_while_condition_not_bool()
    {
        const string code = """
            main {
                var x: int = 1;
                while (x) { print(x); x = x - 1; }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_read_into_bool_variable()
    {
        const string code = """
            main {
                var b: bool;
                read(b);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(1);
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidOperationException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_break_outside_while()
    {
        const string code = """
            main {
                break;
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidLoopControlStatementException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_continue_outside_while()
    {
        const string code = """
            main {
                continue;
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidLoopControlStatementException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_break_in_if_outside_while()
    {
        const string code = """
            main {
                if (true) { break; }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidLoopControlStatementException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_continue_in_if_outside_while()
    {
        const string code = """
            main {
                if (true) { continue; }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidLoopControlStatementException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_string_divided_by_int()
    {
        const string code = """
        main {
            print("hello" / 2);
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_bool_plus_bool()
    {
        const string code = """
        main {
            print(true + false);
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_int_less_than_float()
    {
        const string code = """
        main {
            print(1 < 1.5);
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_bool_equal_to_int()
    {
        const string code = """
        main {
            print(true == 1);
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_logical_and_with_string_operands()
    {
        const string code = """
        main {
            print("hello" && "world");
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_logical_or_with_float_operands()
    {
        const string code = """
        main {
            print(1.0 || 2.0);
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_return_outside_function()
    {
        const string code = """
            main {
                return;
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidReturnStatementException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_return_with_value_outside_function()
    {
        const string code = """
            main {
                return 5;
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidReturnStatementException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_missing_return_in_non_void_function()
    {
        const string code = """
            func f(): int {
                var x: int = 5;
            }
            main {
                print(f());
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<MissingReturnException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_missing_return_when_only_if_without_else()
    {
        const string code = """
            func f(x: int): int {
                if (x > 0) {
                    return x;
                }
            }
            main {
                print(f(5));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<MissingReturnException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_missing_return_when_while_loop_only()
    {
        const string code = """
            func f(x: int): int {
                while (x > 0) {
                    return x;
                }
            }
            main {
                print(f(5));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<MissingReturnException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_unreachable_code_after_return()
    {
        const string code = """
            func f(): int {
                return 1;
                print("unreachable");
            }
            main {
                print(f());
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<UnreachableCodeException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_unreachable_code_after_guaranteed_if_else_return()
    {
        const string code = """
            func f(x: int): int {
                if (x > 0) {
                    return x;
                } else {
                    return 0;
                }
                print("unreachable");
            }
            main {
                print(f(5));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<UnreachableCodeException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_unreachable_code_in_nested_block()
    {
        const string code = """
            func f(): int {
                if (true) {
                    return 1;
                    print("unreachable inside if");
                } else {
                    return 2;
                }
            }
            main {
                print(f());
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<UnreachableCodeException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_void_function_used_as_expression()
    {
        const string code = """
            func greet(): void {
                print("hello");
            }
            main {
                print(greet());
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_void_function_assigned_to_variable()
    {
        const string code = """
            func greet(): void {
                print("hello");
            }
            main {
                var x: int = greet();
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_on_void_function_in_binary_expression()
    {
        const string code = """
            func greet(): void {
                print("hello");
            }
            main {
                var x: int = 1 + greet();
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<TypeErrorException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Void_function_as_statement_is_valid()
    {
        const string code = """
            func greet(): void {
                print("hello");
            }
            main {
                greet();
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Exception? ex = Record.Exception(() => interpreter.Execute(code));
        Assert.Null(ex);
    }

    [Fact]
    public void Non_void_function_with_all_paths_returning_is_valid()
    {
        const string code = """
            func f(x: int): int {
                if (x > 0) {
                    return x;
                } else {
                    return 0;
                }
            }
            main {
                print(f(5));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Exception? ex = Record.Exception(() => interpreter.Execute(code));
        Assert.Null(ex);
    }
}