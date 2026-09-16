using Interpreter;

using Runtime;

using Semantics.Exceptions;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class VariablesTest
{
    [Fact]
    public void Can_declare_and_use_variables()
    {
        const string code = """
            main {
                var x: int = 10;
                var y: int = 20;
                var z: int = x + y;
                print(z);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("30", environment.Output);
    }

    [Fact]
    public void Can_declare_variable_without_initializer()
    {
        const string code = """
            main {
                var x: int;
                x = 42;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("42", environment.Output);
    }

    [Fact]
    public void Uninitialized_variable_has_default_value_zero()
    {
        const string code = """
            main {
                var x: int;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("0", environment.Output);
    }

    [Fact]
    public void Can_reassign_variables()
    {
        const string code = """
            main {
                var x: int = 10;
                x = 25;
                print(x);
                x = x * 2;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("2550", environment.Output);
    }

    [Fact]
    public void Can_use_constants()
    {
        const string code = """
            main {
                const x: int = 100;
                var y: int = x * 2;
                print(y);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("200", environment.Output);
    }

    [Fact]
    public void Cannot_assign_to_constants()
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
    public void Cannot_redeclare_variable_in_same_scope()
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
    public void Can_use_variable_in_expression()
    {
        const string code = """
            main {
                var a: int = 5;
                var b: int = 3;
                var c: int = (a + b) * (a - b);
                print(c);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("16", environment.Output);
    }

    [Fact]
    public void Can_calculate_rectangle_square()
    {
        const string code = """
            main {
                var x1: int = 0;
                var y1: int = 4;
                var x2: int = 6;
                var y2: int = 7;
                var width: int = 0;
                var height: int = 0;
                var square: int = 0;
                
                width = x2 - x1;
                height = y2 - y1;
                square = width * height;
                print(square);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("18", environment.Output);
    }

    [Fact]
    public void Can_declare_and_use_float_variables()
    {
        const string code = """
            main {
                var x: float = 1.5;
                var y: float = 2.0;
                var z: float = x + y;
                print(z);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3.5", environment.Output);
    }

    [Fact]
    public void Can_reassign_float_variable()
    {
        const string code = """
            main {
                var x: float = 1.5;
                x = 3.5;
                print(x);
                x = x * 2.0;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3.57", environment.Output);
    }

    [Fact]
    public void Uninitialized_float_variable_has_default_value_zero()
    {
        const string code = """
            main {
                var x: float;
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("0", environment.Output);
    }

    [Fact]
    public void Can_declare_and_use_string_variables()
    {
        const string code = """
            main {
                var s: string = "hello";
                print(s);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hello", environment.Output);
    }

    [Fact]
    public void Can_concatenate_string_variables()
    {
        const string code = """
            main {
                var a: string = "hello";
                var b: string = " world";
                var c: string = a + b;
                print(c);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hello world", environment.Output);
    }

    [Fact]
    public void Uninitialized_string_variable_has_empty_default_value()
    {
        const string code = """
            main {
                var s: string;
                print(s);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("", environment.Output);
    }

    [Fact]
    public void Can_declare_and_use_bool_variables()
    {
        const string code = """
            main {
                var b: bool = true;
                print(b);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }

    [Fact]
    public void Uninitialized_bool_variable_has_default_value_false()
    {
        const string code = """
            main {
                var b: bool;
                print(b);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("0", environment.Output);
    }

    [Fact]
    public void Can_assign_bool_variable()
    {
        const string code = """
            main {
                var b: bool = false;
                b = true;
                print(b);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }

    [Fact]
    public void Can_use_bool_constant()
    {
        const string code = """
            main {
                const yes: bool = true;
                const no: bool = false;
                print(yes);
                print(no);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("10", environment.Output);
    }

    [Fact]
    public void Bool_false_prints_as_zero()
    {
        const string code = """
            main {
                print(false);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("0", environment.Output);
    }

    [Fact]
    public void Bool_true_prints_as_one()
    {
        const string code = """
            main {
                print(true);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }
}