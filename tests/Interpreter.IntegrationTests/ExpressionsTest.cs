using Interpreter;

using Runtime;

using Semantics.Exceptions;

using Tests.TestLibrary.TestDoubles;

using Xunit;

namespace Interpreter.IntegrationTests;

public class ExpressionsTest
{
    [Theory]
    [MemberData(nameof(GetEvaluateExpressionsData))]
    public void Can_evaluate_expressions(string code, int expected)
    {
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal(expected.ToString(), environment.Output);
    }

    public static TheoryData<string, int> GetEvaluateExpressionsData()
    {
        return new TheoryData<string, int>
        {
            { "main { print(1 + 2 * 8 / 4 - 1); }", 4 },
            { "main { print(10 + 20 * 2); }", 50 },
            { "main { print(100 - 30 / 3); }", 90 },
            { "main { print((1 + 2) * (8 / (3 - 1))); }", 12 },
            { "main { print((10 - 5) * (2 + 3)); }", 25 },
            { "main { print(10 - 3 - 2); }", 5 },
            { "main { print(10 / 2 / 5); }", 1 },
            { "main { print(10 - 3 + 2); }", 9 },
            { "main { print(10 / 5 * 2); }", 4 },
            { "main { print(-4); }", -4 },
            { "main { print(2 * 2 * (-5)); }", -20 },
            { "main { print(-10 + 5); }", -5 },
            { "main { print(10 + -5); }", 5 },
            { "main { print(10 % 3); }", 1 },
            { "main { print(17 % 5); }", 2 },
            { "main { print(20 % 4); }", 0 },
            { "main { print(2 + 3 * 4 - 10 / 2); }", 9 },
            { "main { print((2 + 3) * (4 - 10) / 2); }", -15 },
            { "main { print(1 != 2); }", 1 },
            { "main { print(1 != 1); }", 0 },
            { "main { print(1 == 1); }", 1 },
            { "main { print(1 == 2); }", 0 },
        };
    }

    [Theory]
    [MemberData(nameof(GetEvaluateFloatExpressionsData))]
    public void Can_evaluate_float_expressions(string code, string expected)
    {
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal(expected, environment.Output);
    }

    public static TheoryData<string, string> GetEvaluateFloatExpressionsData()
    {
        return new TheoryData<string, string>
        {
            { "main { print(1.5 + 2.0); }", "3.5" },
            { "main { print(10.0 - 3.5); }", "6.5" },
            { "main { print(3.0 * 2.5); }", "7.5" },
            { "main { print(10.0 / 4.0); }", "2.5" },
            { "main { print(-1.5); }", "-1.5" },
            { "main { print(3.5 % 2.0); }", "1.5" },
            { "main { print(10.0 - 3.0 - 2.5); }", "4.5" },
            { "main { print(10.0 / 2.0 / 2.5); }", "2" },
            { "main { print(10.0 - 3.0 + 2.5); }", "9.5" },
            { "main { print(10.0 / 2.0 * 2.5); }", "12.5" },
            { "main { print(2.0 * 2.0 * (-2.5)); }", "-10" },
            { "main { print(-10.0 + 5.0); }", "-5" },
            { "main { print(10.0 + -5.0); }", "5" },
            { "main { print(1.5 + 2.0 * 3.0); }", "7.5" },
            { "main { print((1.5 + 2.5) * 0.5); }", "2" },
            { "main { print(2.5 + 3.0 * 4.0 - 10.0 / 2.0); }", "9.5" },
            { "main { print((2.0 + 3.0) * (4.0 - 10.0) / 2.0); }", "-15" },
            { "main { print(1.5 == 1.5); }", "1" },
            { "main { print(1.5 != 2.5); }", "1" },
            { "main { print(1.5 == 2.5); }", "0" },
            { "main { print(1.5 != 1.5); }", "0" },
        };
    }

    [Theory]
    [MemberData(nameof(GetEvaluateStringExpressionsData))]
    public void Can_evaluate_string_expressions(string code, string expected)
    {
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal(expected, environment.Output);
    }

    public static TheoryData<string, string> GetEvaluateStringExpressionsData()
    {
        return new TheoryData<string, string>
        {
            { """main { print("hello" + " world"); }""", "hello world" },
            { """main { print("foo" + "bar"); }""", "foobar" },
            { """main { print("a" + "b" + "c"); }""", "abc" },
            { """main { print("x" + "y" + "z" + "w"); }""", "xyzw" },
            { """main { print("hello" + ""); }""", "hello" },
            { """main { print("" + "world"); }""", "world" },
            { """main { print("hello" == "hello"); }""", "1" },
            { """main { print("hello" == "world"); }""", "0" },
            { """main { print("hello" != "world"); }""", "1" },
            { """main { print("hello" != "hello"); }""", "0" },
            { """main { print("" == ""); }""", "1" },
            { """main { print("" != "x"); }""", "1" },
            { """main { print("abc" < "abd"); }""", "1" },
            { """main { print("abc" > "abd"); }""", "0" },
            { """main { print("abc" <= "abc"); }""", "1" },
            { """main { print("xyz" >= "abc"); }""", "1" },
        };
    }

    [Theory]
    [MemberData(nameof(GetRelationalIntExpressionsData))]
    public void Can_evaluate_relational_int_expressions(string code, string expected)
    {
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal(expected, environment.Output);
    }

    public static TheoryData<string, string> GetRelationalIntExpressionsData()
    {
        return new TheoryData<string, string>
        {
            { "main { print(1 < 2); }", "1" },
            { "main { print(2 < 1); }", "0" },
            { "main { print(1 < 1); }", "0" },
            { "main { print(1 > 2); }", "0" },
            { "main { print(2 > 1); }", "1" },
            { "main { print(1 > 1); }", "0" },
            { "main { print(1 <= 2); }", "1" },
            { "main { print(1 <= 1); }", "1" },
            { "main { print(2 <= 1); }", "0" },
            { "main { print(1 >= 2); }", "0" },
            { "main { print(1 >= 1); }", "1" },
            { "main { print(2 >= 1); }", "1" },
        };
    }

    [Theory]
    [MemberData(nameof(GetRelationalFloatExpressionsData))]
    public void Can_evaluate_relational_float_expressions(string code, string expected)
    {
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal(expected, environment.Output);
    }

    public static TheoryData<string, string> GetRelationalFloatExpressionsData()
    {
        return new TheoryData<string, string>
        {
            { "main { print(1.0 < 2.0); }", "1" },
            { "main { print(2.0 < 1.0); }", "0" },
            { "main { print(1.5 <= 1.5); }", "1" },
            { "main { print(2.5 > 1.5); }", "1" },
            { "main { print(1.5 >= 2.5); }", "0" },
        };
    }

    [Theory]
    [MemberData(nameof(GetLogicalExpressionsData))]
    public void Can_evaluate_logical_expressions(string code, string expected)
    {
        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal(expected, environment.Output);
    }

    public static TheoryData<string, string> GetLogicalExpressionsData()
    {
        return new TheoryData<string, string>
        {
            { "main { print(!true); }", "0" },
            { "main { print(!false); }", "1" },
            { "main { print(true && true); }", "1" },
            { "main { print(true && false); }", "0" },
            { "main { print(false && true); }", "0" },
            { "main { print(false && false); }", "0" },
            { "main { print(true || true); }", "1" },
            { "main { print(true || false); }", "1" },
            { "main { print(false || true); }", "1" },
            { "main { print(false || false); }", "0" },
            { "main { print(true == true); }", "1" },
            { "main { print(true == false); }", "0" },
            { "main { print(true != false); }", "1" },
        };
    }

    [Fact]
    public void Logical_and_short_circuits_on_false_left()
    {
        const string code = """
            main {
                var x: int = 0;
                var b: bool = false && (x == 1);
                print(b);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("0", environment.Output);
    }

    [Fact]
    public void Logical_or_short_circuits_on_true_left()
    {
        const string code = """
            main {
                var x: int = 0;
                var b: bool = true || (x == 1);
                print(b);
            }
            """;

        FakeEnvironment environment = new();
        Interpreter interpreter = new(environment);
        interpreter.Execute(code);

        Assert.Equal("1", environment.Output);
    }
}