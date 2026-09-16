using Runtime;

using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class CallBuiltinTest
{
    [Theory]
    [MemberData(nameof(GetPrintData))]
    public void Can_call_print(List<Instruction> program, string expectedOutput)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(0, result);
        Assert.Equal(expectedOutput, env.Output);
    }

    [Theory]
    [MemberData(nameof(GetReadData))]
    public void Can_call_readi(List<Instruction> program, int[] input, string expectedOutput)
    {
        FakeEnvironment env = new FakeEnvironment();
        env.AddInput(input);

        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(0, result);
        Assert.Equal(expectedOutput, env.Output);
    }

    [Fact]
    public void ReadI_returns_value_to_exit_code()
    {
        FakeEnvironment env = new FakeEnvironment();
        env.AddInput(777);

        List<Instruction> program = new List<Instruction>
        {
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadI),
            new Instruction(InstructionCode.Halt),
        };

        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(777, result);
    }

    public static TheoryData<List<Instruction>, string> GetPrintData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // Int
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 123),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "123"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, -456),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "-456"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 10),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 20),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "1020"
            },

            // Float
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(3.14)),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "3.14"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(-2.5)),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "-2.5"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "1.52.5"
            },

            // String
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "hello"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, ""),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                ""
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "foo"),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, "bar"),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "foobar"
            },
        };
    }

    public static TheoryData<List<Instruction>, int[], string> GetReadData()
    {
        return new TheoryData<List<Instruction>, int[], string>
        {
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadI),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                new[] { 42 },
                "42"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadI),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadI),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                new[] { 10, 20 },
                "1020"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetReadFData))]
    public void Can_call_readf(List<Instruction> program, double[] input, string expectedOutput)
    {
        FakeEnvironment env = new FakeEnvironment();
        env.AddFloatInput(input);

        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(0, result);
        Assert.Equal(expectedOutput, env.Output);
    }

    public static TheoryData<List<Instruction>, double[], string> GetReadFData()
    {
        return new TheoryData<List<Instruction>, double[], string>
        {
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadF),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                new[] { 3.5 },
                "3.5"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadF),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadF),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                new[] { 1.5, 2.5 },
                "1.52.5"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetReadSData))]
    public void Can_call_reads(List<Instruction> program, string[] input, string expectedOutput)
    {
        FakeEnvironment env = new FakeEnvironment();
        env.AddStringInput(input);

        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(0, result);
        Assert.Equal(expectedOutput, env.Output);
    }

    public static TheoryData<List<Instruction>, string[], string> GetReadSData()
    {
        return new TheoryData<List<Instruction>, string[], string>
        {
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadS),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                new[] { "hello" },
                "hello"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadS),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.ReadS),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),

                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                new[] { "foo", "bar" },
                "foobar"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetLengthData))]
    public void Can_call_length(List<Instruction> program, string expectedOutput)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(0, result);
        Assert.Equal(expectedOutput, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetLengthData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Length),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "5"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, ""),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Length),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "0"
            },
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "abc"),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Length),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "3"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetSubstrData))]
    public void Can_call_substr(List<Instruction> program, string expectedOutput)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        int result = vm.RunProgram();

        Assert.Equal(0, result);
        Assert.Equal(expectedOutput, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetSubstrData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // substr("hello", 1, 3) = "ell"
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.Push, 3),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Substr),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "ell"
            },

            // substr("hello", 0, 5) = "hello"
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Substr),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "hello"
            },

            // substr("hello", 2, 0) = ""
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.Push, 2),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Substr),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                ""
            },

             // substr("Hello, 🚀", 7, 1) = "🚀"
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "Hello, 🚀"),
                    new Instruction(InstructionCode.Push, 7),
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Substr),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "🚀"
            },
        };
    }
}