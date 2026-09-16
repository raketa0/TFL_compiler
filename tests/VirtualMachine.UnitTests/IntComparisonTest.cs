using Runtime;

using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class IntComparisonTest
{
    [Theory]
    [MemberData(nameof(GetIntEqualData))]
    public void CanCompareIntEqual(List<Instruction> program, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetIntEqualData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // 5 == 5 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Equal),
                ]),
                "1"
            },

            // 5 == 6 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Push, 6),
                    new Instruction(InstructionCode.Equal),
                ]),
                "0"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetIntNotEqualData))]
    public void Can_compare_int_not_equal(List<Instruction> program, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetIntNotEqualData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // 5 != 6 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Push, 6),
                    new Instruction(InstructionCode.NotEqual),
                ]),
                "1"
            },

            // 5 != 5 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.NotEqual),
                ]),
                "0"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetIntLessData))]
    public void Can_compare_int_less(List<Instruction> program, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetIntLessData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // 3 < 5 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 3),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Less),
                ]),
                "1"
            },

            // 5 < 3 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Push, 3),
                    new Instruction(InstructionCode.Less),
                ]),
                "0"
            },

            // 5 < 5 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Less),
                ]),
                "0"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetIntLessOrEqualData))]
    public void Can_compare_int_less_or_equal(List<Instruction> program, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetIntLessOrEqualData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // 3 <= 5 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 3),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.LessOrEqual),
                ]),
                "1"
            },

            // 5 <= 5 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.LessOrEqual),
                ]),
                "1"
            },

            // 6 <= 5 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 6),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.LessOrEqual),
                ]),
                "0"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetFloatLessData))]
    public void Can_compare_float_less(List<Instruction> program, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetFloatLessData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // 1.5 < 2.5 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.Less),
                ]),
                "1"
            },

            // 2.5 < 1.5 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Less),
                ]),
                "0"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetFloatLessOrEqualData))]
    public void Can_compare_float_less_or_equal(List<Instruction> program, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetFloatLessOrEqualData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // 1.5 <= 2.5 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.LessOrEqual),
                ]),
                "1"
            },

            // 2.5 <= 2.5 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.LessOrEqual),
                ]),
                "1"
            },

            // 3.0 <= 2.5 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, new Value(3.0)),
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.LessOrEqual),
                ]),
                "0"
            },
        };
    }

    private static List<Instruction> PrintResult(List<Instruction> setup)
    {
        return [
            ..setup,
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];
    }
}