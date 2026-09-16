using Interpreter;

using Semantics.Exceptions;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class BuiltinFunctionsTest
{
    [Fact]
    public void Can_print_numbers()
    {
        const string code = """
            main {
                print(42);
                print(-10);
                print(0);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("42-100", environment.Output);
    }

    [Fact]
    public void Can_print_expressions()
    {
        const string code = """
            main {
                print(10 + 20);
                print(100 - 30);
                print(15 * 4);
                print(100 / 20);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3070605", environment.Output);
    }

    [Fact]
    public void Can_read_numbers()
    {
        const string code = """
            main {
                var x: int = 0;
                read(x);
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(42);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("42", environment.Output);
    }

    [Fact]
    public void Can_read_multiple_numbers()
    {
        const string code = """
            main {
                var a: int = 0;
                var b: int = 0;
                read(a);
                read(b);
                print(a + b);
                print(a - b);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(100, 30);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("13070", environment.Output);
    }

    [Fact]
    public void Can_read_and_process_in_expression()
    {
        const string code = """
            main {
                var x: int = 0;
                var y: int = 0;
                read(x);
                read(y);
                print(x * y + x - y);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(10, 5);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("55", environment.Output);
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
        environment.AddInput(42);

        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidAssignmentException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Print_can_output_variables()
    {
        const string code = """
            main {
                var a: int = 5;
                var b: int = 3;
                print(a);
                print(b);
                print(a + b);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("538", environment.Output);
    }

    [Fact]
    public void Can_chain_print_and_read()
    {
        const string code = """
            main {
                var x: int = 0;
                print(1);
                read(x);
                print(x);
                print(2);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddInput(99);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1992", environment.Output);
    }

    [Fact]
    public void Throws_when_no_input_available()
    {
        const string code = """
            main {
                var x: int = 0;
                read(x);
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<InvalidOperationException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Can_read_float()
    {
        const string code = """
            main {
                var x: float = 0.0;
                read(x);
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddFloatInput(3.5);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3.5", environment.Output);
    }

    [Fact]
    public void Can_read_multiple_floats()
    {
        const string code = """
            main {
                var a: float = 0.0;
                var b: float = 0.0;
                read(a);
                read(b);
                print(a + b);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddFloatInput(1.5, 2.0);

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3.5", environment.Output);
    }

    [Fact]
    public void Can_read_string()
    {
        const string code = """
            main {
                var s: string = "";
                read(s);
                print(s);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddStringInput("hello");

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hello", environment.Output);
    }

    [Fact]
    public void Can_read_multiple_strings()
    {
        const string code = """
            main {
                var a: string = "";
                var b: string = "";
                read(a);
                read(b);
                print(a + b);
            }
            """;

        FakeEnvironment environment = new();
        environment.AddStringInput("hello", " world");

        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hello world", environment.Output);
    }

    [Fact]
    public void Substr_returns_middle_of_string()
    {
        const string code = """
            main {
                print(substr("hello", 1, 3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("ell", environment.Output);
    }

    [Fact]
    public void Substr_returns_prefix()
    {
        const string code = """
            main {
                print(substr("hello", 0, 3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hel", environment.Output);
    }

    [Fact]
    public void Substr_returns_suffix()
    {
        const string code = """
            main {
                print(substr("hello", 2, 3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("llo", environment.Output);
    }

    [Fact]
    public void Substr_returns_empty_string_when_length_is_zero()
    {
        const string code = """
            main {
                print(substr("hello", 2, 0));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("", environment.Output);
    }

    [Fact]
    public void Substr_works_with_string_variable()
    {
        const string code = """
            main {
                var s: string = "world";
                print(substr(s, 1, 3));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("orl", environment.Output);
    }

    [Fact]
    public void Substr_works_with_int_variables_for_start_and_length()
    {
        const string code = """
            main {
                var s: string = "abcdef";
                var start: int = 2;
                var len: int = 3;
                print(substr(s, start, len));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("cde", environment.Output);
    }

    [Fact]
    public void Substr_combined_with_length()
    {
        const string code = """
            main {
                var s: string = "hello";
                print(substr(s, 0, length(s) - 2));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hel", environment.Output);
    }

    [Fact]
    public void Substr_result_can_be_assigned_to_variable()
    {
        const string code = """
            main {
                var s: string = "programming";
                var sub: string = substr(s, 0, 7);
                print(sub);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("program", environment.Output);
    }

    [Fact]
    public void Substr_result_can_be_concatenated()
    {
        const string code = """
            main {
                var s: string = "hello world";
                print(substr(s, 0, 5) + "!");
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("hello!", environment.Output);
    }

    [Fact]
    public void Length_returns_string_length()
    {
        const string code = """
            main {
                print(length("hello"));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("5", environment.Output);
    }

    [Fact]
    public void Length_of_empty_string_is_zero()
    {
        const string code = """
            main {
                print(length(""));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("0", environment.Output);
    }

    [Fact]
    public void Length_works_with_string_variable()
    {
        const string code = """
            main {
                var s: string = "abc";
                print(length(s));
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3", environment.Output);
    }

    [Fact]
    public void Substr_handles_unicode_characters_correctly()
    {
        const string code = """
        main {
            print(substr("Hello, 🚀", 7, 1));
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("🚀", environment.Output);
    }

    [Fact]
    public void Length_counts_unicode_symbols_not_utf16_units()
    {
        const string code = """
        main {
            print(length("🗿🗿🗿"));
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3", environment.Output);
    }

    [Fact]
    public void Substr_with_unicode_emoji_uses_symbol_positions()
    {
        const string code = """
        main {
            print(substr("🗿🗿🗿", 1, 2));
        }
        """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("🗿🗿", environment.Output);
    }
}