using Runtime;

using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class CallReturnTest
{
    [Fact]
    public void Call_jumps_to_function_and_Return_resumes_caller()
    {
        FakeEnvironment env = new FakeEnvironment();

        // func constant(): int { return 42; }
        // main: var x = constant(); print(x);
        //
        // [0] Call 4         → jump to function at index 4
        // [1] CallBuiltin Print
        // [2] Push 0
        // [3] Halt
        // [4] PushVars 0     ← function start
        // [5] Push 42
        // [6] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Call, 4),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.PushVars, 0),
            new Instruction(InstructionCode.Push, 42),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("42", env.Output);
    }

    [Fact]
    public void Call_with_one_argument_defines_param_and_returns_value()
    {
        FakeEnvironment env = new FakeEnvironment();

        // func double(n: int): int { return n + n; }
        // main: print(double(5));
        //
        // [0] Push 5         ← argument
        // [1] Call 5         → jump to function at index 5
        // [2] CallBuiltin Print
        // [3] Push 0
        // [4] Halt
        // [5] PushVars 0     ← function start
        // [6] DefineVar "n"  ← pops 5
        // [7] LoadVar "n"
        // [8] LoadVar "n"
        // [9] Add
        // [10] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 5),
            new Instruction(InstructionCode.Call, 5),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.PushVars, 0),
            new Instruction(InstructionCode.DefineVar, "n"),
            new Instruction(InstructionCode.LoadVar, "n"),
            new Instruction(InstructionCode.LoadVar, "n"),
            new Instruction(InstructionCode.Add),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("10", env.Output);
    }

    [Fact]
    public void Call_with_two_arguments_defines_params_in_correct_order()
    {
        FakeEnvironment env = new FakeEnvironment();

        // func add(a: int, b: int): int { return a + b; }
        // main: print(add(3, 4));
        //
        // [0] Push 3          ← arg a (pushed first)
        // [1] Push 4          ← arg b (pushed second, on top)
        // [2] Call 7
        // [3] CallBuiltin Print
        // [4] Push 0
        // [5] Halt
        // [6] (never reached from main, but valid dead code)
        // [7] PushVars 0
        // [8] DefineVar "b"   ← pops 4 (top)
        // [9] DefineVar "a"   ← pops 3
        // [10] LoadVar "a"
        // [11] LoadVar "b"
        // [12] Add
        // [13] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 3),
            new Instruction(InstructionCode.Push, 4),
            new Instruction(InstructionCode.Call, 7),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.Push, 0),  // padding to align function at index 7
            new Instruction(InstructionCode.PushVars, 0),
            new Instruction(InstructionCode.DefineVar, "b"),
            new Instruction(InstructionCode.DefineVar, "a"),
            new Instruction(InstructionCode.LoadVar, "a"),
            new Instruction(InstructionCode.LoadVar, "b"),
            new Instruction(InstructionCode.Add),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("7", env.Output);
    }

    [Fact]
    public void Return_restores_caller_variables()
    {
        FakeEnvironment env = new FakeEnvironment();

        // Verify that after a function call, the caller's variables are still accessible.
        //
        // [0] Push 99
        // [1] DefineVar "outer"
        // [2] Call 8          → call no-op function
        // [3] LoadVar "outer" ← must still be accessible after return
        // [4] CallBuiltin Print
        // [5] Push 0
        // [6] Halt
        // [7] (padding)
        // [8] PushVars 0
        // [9] Push 1          (dummy return value)
        // [10] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 99),
            new Instruction(InstructionCode.DefineVar, "outer"),
            new Instruction(InstructionCode.Call, 8),
            new Instruction(InstructionCode.LoadVar, "outer"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.Push, 0),  // padding
            new Instruction(InstructionCode.PushVars, 0),
            new Instruction(InstructionCode.Push, 1),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("99", env.Output);
    }

    [Fact]
    public void Function_can_be_called_multiple_times()
    {
        FakeEnvironment env = new FakeEnvironment();

        // func inc(n: int): int { return n + 1; }
        // main: print(inc(1)); print(inc(2)); print(inc(3));
        //
        // [0] Push 1
        // [1] Call 13
        // [2] CallBuiltin Print
        // [3] Push 2
        // [4] Call 13
        // [5] CallBuiltin Print
        // [6] Push 3
        // [7] Call 13
        // [8] CallBuiltin Print
        // [9] Push 0
        // [10] Halt
        // [11..12] padding
        // [13] PushVars 0
        // [14] DefineVar "n"
        // [15] LoadVar "n"
        // [16] Push 1
        // [17] Add
        // [18] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 1),
            new Instruction(InstructionCode.Call, 13),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 2),
            new Instruction(InstructionCode.Call, 13),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 3),
            new Instruction(InstructionCode.Call, 13),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.Push, 0),  // padding
            new Instruction(InstructionCode.Push, 0),  // padding
            new Instruction(InstructionCode.PushVars, 0),
            new Instruction(InstructionCode.DefineVar, "n"),
            new Instruction(InstructionCode.LoadVar, "n"),
            new Instruction(InstructionCode.Push, 1),
            new Instruction(InstructionCode.Add),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("234", env.Output);
    }

    [Fact]
    public void Call_return_stack_is_restored_correctly_after_nested_calls()
    {
        FakeEnvironment env = new FakeEnvironment();

        // outer() calls inner() and adds 10 to the result.
        // inner() returns 5.
        // main: print(outer()); → expects 15
        //
        // [0] Call 5         → call outer
        // [1] CallBuiltin Print
        // [2] Push 0
        // [3] Halt
        // [4] padding
        //
        // outer function at [5]:
        // [5] PushVars 0
        // [6] Call 13        → call inner
        // [7] Push 10
        // [8] Add
        // [9] Return
        // [10..12] padding
        //
        // inner function at [13]:
        // [13] PushVars 0
        // [14] Push 5
        // [15] Return
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Call, 5),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
            new Instruction(InstructionCode.Push, 0),  // padding
            new Instruction(InstructionCode.PushVars, 0),   // outer start [5]
            new Instruction(InstructionCode.Call, 13),
            new Instruction(InstructionCode.Push, 10),
            new Instruction(InstructionCode.Add),
            new Instruction(InstructionCode.Return),
            new Instruction(InstructionCode.Push, 0),  // padding [10]
            new Instruction(InstructionCode.Push, 0),  // padding [11]
            new Instruction(InstructionCode.Push, 0),  // padding [12]
            new Instruction(InstructionCode.PushVars, 0),   // inner start [13]
            new Instruction(InstructionCode.Push, 5),
            new Instruction(InstructionCode.Return),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("15", env.Output);
    }
}