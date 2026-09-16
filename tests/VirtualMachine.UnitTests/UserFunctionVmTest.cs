using Runtime;

using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class UserFunctionVmTest
{
    [Fact]
    public void Void_function_return_pushes_void_on_stack()
    {
        FakeEnvironment env = new();

        // func greet(): void { }
        // main: greet(); — the void result is immediately discarded (Pop)
        //
        // [0] Call 4          → jump to void function
        // [1] Pop             ← discard the void return value
        // [2] Push 0
        // [3] Halt
        // [4] PushVars 0      ← function start
        // [5] Push Void
        // [6] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Call, 4),
            new Instruction(InstructionCode.Pop),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.PushVars, 0),
            new Instruction(InstructionCode.Push, Value.Void),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new(env, program);
        int exitCode = vm.RunProgram();

        Assert.Equal(0, exitCode);
    }

    [Fact]
    public void Function_return_value_propagates_through_add()
    {
        FakeEnvironment env = new();

        // func one(): int { return 1; }
        // main: print(one() + 41); → "42"
        //
        // [0] Call 6          → call one()
        // [1] Push 41
        // [2] Add             ← 1 + 41 = 42
        // [3] CallBuiltin Print
        // [4] Push 0
        // [5] Halt
        // [6] PushVars 0      ← one() start
        // [7] Push 1
        // [8] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Call, 6),
            new Instruction(InstructionCode.Push, 41),
            new Instruction(InstructionCode.Add),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.PushVars, 0),
            new Instruction(InstructionCode.Push, 1),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new(env, program);
        vm.RunProgram();

        Assert.Equal("42", env.Output);
    }

    [Fact]
    public void Function_with_conditional_branch_returns_correct_value()
    {
        FakeEnvironment env = new();

        // func abs(n: int): int {
        //     if (n < 0) { return -n; } else { return n; }
        // }
        // main: print(abs(-5)); → "5"
        //
        // [0] Push -5
        // [1] Call 7
        // [2] CallBuiltin Print
        // [3] Push 0
        // [4] Halt
        // [5] padding
        // [6] padding
        //
        // abs at [7]:
        // [7]  PushVars 0
        // [8]  DefineVar "n"
        // [9]  LoadVar "n"
        // [10] Push 0
        // [11] Less               ← n < 0
        // [12] JumpIfFalse 16     → else branch at [16]
        // [13] LoadVar "n"
        // [14] Negate
        // [15] Return             ← return -n
        // [16] LoadVar "n"
        // [17] Return             ← return n
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, -5),
            new Instruction(InstructionCode.Call, 7),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.Push, 0),  // padding
            new Instruction(InstructionCode.Push, 0),  // padding
            new Instruction(InstructionCode.PushVars, 0),   // abs start [7]
            new Instruction(InstructionCode.DefineVar, "n"),
            new Instruction(InstructionCode.LoadVar, "n"),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Less),
            new Instruction(InstructionCode.JumpIfFalse, 16),
            new Instruction(InstructionCode.LoadVar, "n"),
            new Instruction(InstructionCode.Negate),
            new Instruction(InstructionCode.Return),
            new Instruction(InstructionCode.LoadVar, "n"),  // [16]
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new(env, program);
        vm.RunProgram();

        Assert.Equal("5", env.Output);
    }

    [Fact]
    public void Function_local_variable_does_not_affect_caller_scope()
    {
        FakeEnvironment env = new();

        // Caller defines x = 10. Function defines its own x = 99.
        // After return, caller's x must still be 10.
        //
        // [0] Push 10
        // [1] DefineVar "x"
        // [2] Call 9          → call func
        // [3] Pop             ← discard return value
        // [4] LoadVar "x"     ← must be 10, not 99
        // [5] CallBuiltin Print
        // [6] Push 0
        // [7] Halt
        // [8] padding
        //
        // func at [9]:
        // [9]  PushVars 0
        // [10] Push 99
        // [11] DefineVar "x"  ← local x = 99
        // [12] Push 1
        // [13] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 10),
            new Instruction(InstructionCode.DefineVar, "x"),
            new Instruction(InstructionCode.Call, 9),
            new Instruction(InstructionCode.Pop),
            new Instruction(InstructionCode.LoadVar, "x"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.Push, 0),  // padding
            new Instruction(InstructionCode.PushVars, 0),   // func start [9]
            new Instruction(InstructionCode.Push, 99),
            new Instruction(InstructionCode.DefineVar, "x"),
            new Instruction(InstructionCode.Push, 1),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new(env, program);
        vm.RunProgram();

        Assert.Equal("10", env.Output);
    }

    [Fact]
    public void Three_level_nested_calls_restore_stack_correctly()
    {
        FakeEnvironment env = new();

        // a() calls b() which calls c().
        // c returns 1, b returns c() + 2 = 3, a returns b() + 4 = 7.
        // main: print(a()); → "7"
        //
        // [0] Call 5          → a
        // [1] CallBuiltin Print
        // [2] Push 0
        // [3] Halt
        // [4] padding
        //
        // a at [5]:
        // [5] PushVars 0
        // [6] Call 12         → b
        // [7] Push 4
        // [8] Add
        // [9] Return
        // [10..11] padding
        //
        // b at [12]:
        // [12] PushVars 0
        // [13] Call 18        → c
        // [14] Push 2
        // [15] Add
        // [16] Return
        // [17] padding
        //
        // c at [18]:
        // [18] PushVars 0
        // [19] Push 1
        // [20] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Call, 5),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.Push, 0),      // padding [4]
            new Instruction(InstructionCode.PushVars, 0),  // a [5]
            new Instruction(InstructionCode.Call, 12),
            new Instruction(InstructionCode.Push, 4),
            new Instruction(InstructionCode.Add),
            new Instruction(InstructionCode.Return),
            new Instruction(InstructionCode.Push, 0),      // padding [10]
            new Instruction(InstructionCode.Push, 0),      // padding [11]
            new Instruction(InstructionCode.PushVars, 0),  // b [12]
            new Instruction(InstructionCode.Call, 18),
            new Instruction(InstructionCode.Push, 2),
            new Instruction(InstructionCode.Add),
            new Instruction(InstructionCode.Return),
            new Instruction(InstructionCode.Push, 0),      // padding [17]
            new Instruction(InstructionCode.PushVars, 0),  // c [18]
            new Instruction(InstructionCode.Push, 1),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new(env, program);
        vm.RunProgram();

        Assert.Equal("7", env.Output);
    }
}