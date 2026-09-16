using Runtime;

using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class EvaluationTest
{
    [Theory]
    [MemberData(nameof(GetEvaluateExpressionData))]
    public void Can_evaluate_expression(List<Instruction> instructions, string expected)
    {
        FakeEnvironment env = new FakeEnvironment();
        VirtualMachine vm = new VirtualMachine(env, instructions);

        vm.RunProgram();

        Assert.Equal(0, vm.ExitCode);
        Assert.Equal(expected, env.Output);
    }

    public static TheoryData<List<Instruction>, string> GetEvaluateExpressionData()
    {
        return new TheoryData<List<Instruction>, string>
        {
            // Push + Print
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 42),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "42"
            },

            // +
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 10),
                    new Instruction(InstructionCode.Push, 20),
                    new Instruction(InstructionCode.Add),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "30"
            },

            // -
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 50),
                    new Instruction(InstructionCode.Push, 30),
                    new Instruction(InstructionCode.Subtract),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "20"
            },

            // *
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 7),
                    new Instruction(InstructionCode.Push, 6),
                    new Instruction(InstructionCode.Multiply),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "42"
            },

            // /
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 100),
                    new Instruction(InstructionCode.Push, 4),
                    new Instruction(InstructionCode.Divide),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "25"
            },

            // %
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 17),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Modulo),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "2"
            },

            // Negate
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 42),
                    new Instruction(InstructionCode.Negate),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "-42"
            },

            // Pop
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 10),
                    new Instruction(InstructionCode.Push, 20),
                    new Instruction(InstructionCode.Pop),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "10"
            },

            // (10 + 5) * 3
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 10),
                    new Instruction(InstructionCode.Push, 5),
                    new Instruction(InstructionCode.Add),
                    new Instruction(InstructionCode.Push, 3),
                    new Instruction(InstructionCode.Multiply),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "45"
            },

            // (100 / 4) + (2 * 3) * 2 = 25 + 12 = 37
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, 100),
                    new Instruction(InstructionCode.Push, 4),
                    new Instruction(InstructionCode.Divide),

                    new Instruction(InstructionCode.Push, 2),
                    new Instruction(InstructionCode.Push, 3),
                    new Instruction(InstructionCode.Multiply),

                    new Instruction(InstructionCode.Push, 2),
                    new Instruction(InstructionCode.Multiply),

                    new Instruction(InstructionCode.Add),

                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "37"
            },

            // Float +
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Push, new Value(2.0)),
                    new Instruction(InstructionCode.Add),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "3.5"
            },

            // Float -
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(10.0)),
                    new Instruction(InstructionCode.Push, new Value(3.5)),
                    new Instruction(InstructionCode.Subtract),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "6.5"
            },

            // Float *
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(3.0)),
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.Multiply),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "7.5"
            },

            // Float /
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(10.0)),
                    new Instruction(InstructionCode.Push, new Value(4.0)),
                    new Instruction(InstructionCode.Divide),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "2.5"
            },

            // Float %
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(3.5)),
                    new Instruction(InstructionCode.Push, new Value(2.0)),
                    new Instruction(InstructionCode.Modulo),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "1.5"
            },

            // Float Negate
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Negate),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "-1.5"
            },

            // Float Equal (true)
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Equal),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "1"
            },

            // Float Equal (false)
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.Equal),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "0"
            },

            // Float NotEqual (true)
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.NotEqual),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "1"
            },

            // Float NotEqual (false)
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.NotEqual),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "0"
            },

            // Float compound: (1.5 + 2.5) * 2.0 = 8.0
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, new Value(1.5)),
                    new Instruction(InstructionCode.Push, new Value(2.5)),
                    new Instruction(InstructionCode.Add),
                    new Instruction(InstructionCode.Push, new Value(2.0)),
                    new Instruction(InstructionCode.Multiply),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "8"
            },

            // String +
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.Push, " world"),
                    new Instruction(InstructionCode.Add),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "hello world"
            },

            // String + (chain)
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "a"),
                    new Instruction(InstructionCode.Push, "b"),
                    new Instruction(InstructionCode.Add),
                    new Instruction(InstructionCode.Push, "c"),
                    new Instruction(InstructionCode.Add),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "abc"
            },

            // String Equal (true)
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.Equal),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "1"
            },

            // String Equal (false)
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.Push, "world"),
                    new Instruction(InstructionCode.Equal),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "0"
            },

            // String NotEqual (true)
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.Push, "world"),
                    new Instruction(InstructionCode.NotEqual),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "1"
            },

            // String NotEqual (false)
            {
                new List<Instruction>
                {
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.Push, "hello"),
                    new Instruction(InstructionCode.NotEqual),
                    new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
                    new Instruction(InstructionCode.Push, 0),
                    new Instruction(InstructionCode.Halt),
                },
                "0"
            },
        };
    }
}