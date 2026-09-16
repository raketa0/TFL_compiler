using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class LogicalOperationTest
{
    [Theory]
    [MemberData(nameof(GetAndData))]
    public void Can_apply_logical_and(List<Instruction> program, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetAndData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // 1 && 1 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.And),
                ]),
                "1"
            },

            // 1 && 0 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.And),
                ]),
                "0"
            },

            // 0 && 1 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.And),
                ]),
                "0"
            },

            // 0 && 0 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.And),
                ]),
                "0"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetOrData))]
    public void Can_apply_logical_or(List<Instruction> program, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetOrData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // 1 || 0 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Or),
                ]),
                "1"
            },

            // 0 || 1 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.Or),
                ]),
                "1"
            },

            // 1 || 1 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.Or),
                ]),
                "1"
            },

            // 0 || 0 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Or),
                ]),
                "0"
            },
        };
    }

    [Theory]
    [MemberData(nameof(GetNotData))]
    public void Can_apply_logical_not(List<Instruction> program, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetNotData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // !0 → "1"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Not),
                ]),
                "1"
            },

            // !1 → "0"
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.Not),
                ]),
                "0"
            },

            // !42 → "0" (любое ненулевое значение)
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 42),
                    new Instruction(InstructionCode.Not),
                ]),
                "0"
            },

            // !!1 → "1" (двойное отрицание)
            {
                PrintResult([
                    new Instruction(InstructionCode.Push, 1),
                    new Instruction(InstructionCode.Not),
                    new Instruction(InstructionCode.Not),
                ]),
                "1"
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