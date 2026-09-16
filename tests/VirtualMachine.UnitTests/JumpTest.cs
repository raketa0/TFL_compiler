using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class JumpTest
{
    [Fact]
    public void Jump_skips_instructions()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 42),
            new Instruction(InstructionCode.Jump, 3),            // перейти к индексу 3
            new Instruction(InstructionCode.Push, 99),           // пропускается
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("42", env.Output);
    }

    [Fact]
    public void JumpIfTrue_jumps_when_condition_is_nonzero()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 1),            // условие = true
            new Instruction(InstructionCode.JumpIfTrue, 4),      // перейти к индексу 4
            new Instruction(InstructionCode.Push, 0),            // пропускается
            new Instruction(InstructionCode.Jump, 5),            // пропускается
            new Instruction(InstructionCode.Push, 42),           // сюда прыгаем
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("42", env.Output);
    }

    [Fact]
    public void JumpIfTrue_does_not_jump_when_condition_is_zero()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 0),            // условие = false
            new Instruction(InstructionCode.JumpIfTrue, 4),      // не прыгаем
            new Instruction(InstructionCode.Push, 99),           // выполняется
            new Instruction(InstructionCode.Jump, 5),
            new Instruction(InstructionCode.Push, 0),            // пропускается
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("99", env.Output);
    }

    [Fact]
    public void JumpIfFalse_jumps_when_condition_is_zero()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 0),            // условие = false
            new Instruction(InstructionCode.JumpIfFalse, 4),     // перейти к индексу 4
            new Instruction(InstructionCode.Push, 0),            // пропускается
            new Instruction(InstructionCode.Jump, 5),            // пропускается
            new Instruction(InstructionCode.Push, 42),           // сюда прыгаем
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("42", env.Output);
    }

    [Fact]
    public void JumpIfFalse_does_not_jump_when_condition_is_nonzero()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 1),            // условие = true
            new Instruction(InstructionCode.JumpIfFalse, 4),     // не прыгаем
            new Instruction(InstructionCode.Push, 99),           // выполняется
            new Instruction(InstructionCode.Jump, 5),
            new Instruction(InstructionCode.Push, 0),            // пропускается
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("99", env.Output);
    }

    [Fact]
    public void Can_implement_if_else_with_jumps()
    {
        FakeEnvironment env = new FakeEnvironment();

        // if (1 < 5) { print 1 } else { print 0 }
        //
        // [0] Push 1
        // [1] Push 5
        // [2] Less
        // [3] JumpIfFalse(6)  → else-ветка
        // [4] Push 1          → then-ветка
        // [5] Jump(7)         → пропустить else
        // [6] Push 0          → else-ветка
        // [7] CallBuiltin(Print)
        // [8] Push 0
        // [9] Halt
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 1),
            new Instruction(InstructionCode.Push, 5),
            new Instruction(InstructionCode.Less),
            new Instruction(InstructionCode.JumpIfFalse, 6),
            new Instruction(InstructionCode.Push, 1),
            new Instruction(InstructionCode.Jump, 7),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("1", env.Output);
    }
}