using Interpreter;

using Parsing;

using Runtime;

using Semantics.Exceptions;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class EntryPointTest
{
    [Fact]
    public void Can_execute_main_block()
    {
        const string code = """
            main {
                print(100);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("100", environment.Output);
    }

    [Fact]
    public void Can_execute_main_with_multiple_statements()
    {
        const string code = """
            main {
                var x: int = 10;
                var y: int = 20;
                print(x + y);
                print(x * y);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("30200", environment.Output);
    }

    [Fact]
    public void Throws_on_missing_main()
    {
        const string code = """
            {
                print(10);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<UnexpectedLexemeException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Throws_when_main_is_not_first()
    {
        const string code = """
            var x: int = 10;
            main {
                print(x);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);

        Assert.Throws<UnexpectedLexemeException>(() => interpreter.Execute(code));
    }

    [Fact]
    public void Main_can_have_empty_body()
    {
        const string code = """
            main {
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("", environment.Output);
    }
}