using Interpreter;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class ControlFlowTest
{
    [Fact]
    public void If_executes_then_branch_when_condition_true()
    {
        const string code = """
            main {
                if (true) { print(1); }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }

    [Fact]
    public void If_skips_body_when_condition_false()
    {
        const string code = """
            main {
                if (false) { print(1); }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("", environment.Output);
    }

    [Fact]
    public void If_else_executes_then_branch()
    {
        const string code = """
            main {
                var x: int = 5;
                if (x > 3) { print(1); } else { print(0); }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }

    [Fact]
    public void If_else_executes_else_branch()
    {
        const string code = """
            main {
                var x: int = 2;
                if (x > 3) { print(1); } else { print(0); }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("0", environment.Output);
    }

    [Fact]
    public void Else_if_chain_takes_first_true_branch()
    {
        const string code = """
            main {
                var x: int = 5;
                if (x > 10) {
                    print(3);
                } else if (x > 3) {
                    print(2);
                } else {
                    print(1);
                }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("2", environment.Output);
    }

    [Fact]
    public void Else_if_chain_takes_else_branch()
    {
        const string code = """
            main {
                var x: int = 1;
                if (x > 10) {
                    print(3);
                } else if (x > 3) {
                    print(2);
                } else {
                    print(1);
                }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }

    [Fact]
    public void If_with_comparison_expression()
    {
        const string code = """
            main {
                var a: int = 10;
                var b: int = 20;
                if (a < b) { print(1); }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }

    [Fact]
    public void While_executes_body_while_condition_true()
    {
        const string code = """
            main {
                var i: int = 0;
                while (i < 3) {
                    print(i);
                    i = i + 1;
                }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("012", environment.Output);
    }

    [Fact]
    public void While_does_not_execute_when_condition_initially_false()
    {
        const string code = """
            main {
                var i: int = 10;
                while (i < 3) {
                    print(i);
                    i = i + 1;
                }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("", environment.Output);
    }

    [Fact]
    public void While_with_break_exits_loop()
    {
        const string code = """
            main {
                var i: int = 0;
                while (true) {
                    if (i >= 3) { break; }
                    print(i);
                    i = i + 1;
                }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("012", environment.Output);
    }

    [Fact]
    public void While_with_continue_skips_rest_of_body()
    {
        const string code = """
            main {
                var i: int = 0;
                while (i < 5) {
                    i = i + 1;
                    if (i == 3) { continue; }
                    print(i);
                }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1245", environment.Output);
    }

    [Fact]
    public void Nested_while_loops()
    {
        const string code = """
            main {
                var i: int = 0;
                while (i < 2) {
                    var j: int = 0;
                    while (j < 2) {
                        print(i);
                        print(j);
                        j = j + 1;
                    }
                    i = i + 1;
                }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("00011011", environment.Output);
    }

    [Fact]
    public void While_with_bool_variable_as_condition()
    {
        const string code = """
            main {
                var running: bool = true;
                var count: int = 0;
                while (running) {
                    count = count + 1;
                    if (count >= 3) { running = false; }
                }
                print(count);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("3", environment.Output);
    }

    [Fact]
    public void If_with_logical_and()
    {
        const string code = """
            main {
                var x: int = 5;
                if (x > 3 && x < 10) { print(1); } else { print(0); }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }

    [Fact]
    public void If_with_logical_or()
    {
        const string code = """
            main {
                var x: int = 1;
                if (x > 10 || x < 5) { print(1); } else { print(0); }
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }
}