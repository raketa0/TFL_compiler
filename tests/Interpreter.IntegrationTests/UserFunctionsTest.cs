using Interpreter;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class UserFunctionsTest
{
    [Fact]
    public void Function_returning_int_value()
    {
        const string code = """
            func double(n: int): int {
                return n + n;
            }
            main {
                print(double(5));
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("10", env.Output);
    }

    [Fact]
    public void Void_function_executes_side_effects()
    {
        const string code = """
            func greet(): void {
                print("hello");
            }
            main {
                greet();
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("hello", env.Output);
    }

    [Fact]
    public void Function_with_two_int_parameters()
    {
        const string code = """
            func add(a: int, b: int): int {
                return a + b;
            }
            main {
                print(add(3, 4));
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("7", env.Output);
    }

    [Fact]
    public void Function_called_multiple_times()
    {
        const string code = """
            func inc(n: int): int {
                return n + 1;
            }
            main {
                print(inc(1));
                print(inc(2));
                print(inc(3));
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("234", env.Output);
    }

    [Fact]
    public void Nested_function_calls()
    {
        const string code = """
            func inner(): int {
                return 5;
            }
            func outer(): int {
                return inner() + 10;
            }
            main {
                print(outer());
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("15", env.Output);
    }

    [Fact]
    public void Function_with_local_variables()
    {
        const string code = """
            func compute(x: int): int {
                var result: int = x * 2;
                result = result + 1;
                return result;
            }
            main {
                print(compute(5));
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("11", env.Output);
    }

    [Fact]
    public void Function_with_if_else_branch()
    {
        const string code = """
            func max(a: int, b: int): int {
                if (a > b) {
                    return a;
                } else {
                    return b;
                }
            }
            main {
                print(max(3, 7));
                print(max(10, 4));
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("710", env.Output);
    }

    [Fact]
    public void Function_result_used_in_expression()
    {
        const string code = """
            func double(n: int): int {
                return n + n;
            }
            main {
                print(double(3) + 1);
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("7", env.Output);
    }

    [Fact]
    public void Multiple_functions_defined_and_called()
    {
        const string code = """
            func square(n: int): int {
                return n * n;
            }
            func cube(n: int): int {
                return n * n * n;
            }
            main {
                print(square(3));
                print(cube(2));
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("98", env.Output);
    }

    [Fact]
    public void Caller_variables_not_modified_by_function()
    {
        const string code = """
            func inc(n: int): int {
                return n + 1;
            }
            main {
                var x: int = 10;
                var result: int = inc(5);
                print(x);
                print(result);
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("106", env.Output);
    }

    [Fact]
    public void Function_with_float_parameters()
    {
        const string code = """
            func halve(x: float): float {
                return x * 0.5;
            }
            main {
                print(halve(10.0));
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("5", env.Output);
    }

    [Fact]
    public void Function_with_string_parameter_and_return()
    {
        const string code = """
            func greet(name: string): string {
                return "hello " + name;
            }
            main {
                print(greet("world"));
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("hello world", env.Output);
    }

    [Fact]
    public void Function_with_while_loop()
    {
        const string code = """
            func sumTo(n: int): int {
                var sum: int = 0;
                var i: int = 1;
                while (i <= n) {
                    sum = sum + i;
                    i = i + 1;
                }
                return sum;
            }
            main {
                print(sumTo(4));
            }
            """;

        FakeEnvironment env = new();
        new Interpreter(env).Execute(code);

        Assert.Equal("10", env.Output);
    }
}