using Runtime;

using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class VariableTest
{
    [Fact]
    public void Can_define_and_load_variable()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 42),
            new Instruction(InstructionCode.DefineVar, "x"),
            new Instruction(InstructionCode.LoadVar, "x"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("42", env.Output);
    }

    [Fact]
    public void Can_store_new_value_in_variable()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 10),
            new Instruction(InstructionCode.DefineVar, "x"),
            new Instruction(InstructionCode.Push, 20),
            new Instruction(InstructionCode.StoreVar, "x"),
            new Instruction(InstructionCode.LoadVar, "x"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("20", env.Output);
    }

    [Fact]
    public void Can_use_multiple_variables()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 3),
            new Instruction(InstructionCode.DefineVar, "a"),
            new Instruction(InstructionCode.Push, 4),
            new Instruction(InstructionCode.DefineVar, "b"),
            new Instruction(InstructionCode.LoadVar, "a"),
            new Instruction(InstructionCode.LoadVar, "b"),
            new Instruction(InstructionCode.Add),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("7", env.Output);
    }

    [Fact]
    public void Can_define_variable_in_nested_scope()
    {
        FakeEnvironment env = new FakeEnvironment();

        // Outer scope at depth 1, inner scope at depth 2 (parent = depth 1)
        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 10),
            new Instruction(InstructionCode.DefineVar, "outer"),
            new Instruction(InstructionCode.PushVars, 1),          // depth 2, parent = depth 1
            new Instruction(InstructionCode.Push, 20),
            new Instruction(InstructionCode.DefineVar, "inner"),
            new Instruction(InstructionCode.LoadVar, "inner"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.LoadVar, "outer"),     // visible through parent
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.PopVars),
            new Instruction(InstructionCode.LoadVar, "outer"),     // still accessible in outer
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("201010", env.Output);
    }

    [Fact]
    public void Can_store_to_variable_in_outer_scope()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, 1),
            new Instruction(InstructionCode.DefineVar, "x"),
            new Instruction(InstructionCode.PushVars, 1),
            new Instruction(InstructionCode.Push, 99),
            new Instruction(InstructionCode.StoreVar, "x"),        // assign to outer x
            new Instruction(InstructionCode.PopVars),
            new Instruction(InstructionCode.LoadVar, "x"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("99", env.Output);
    }

    [Fact]
    public void Can_define_string_variable()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, "hello"),
            new Instruction(InstructionCode.DefineVar, "s"),
            new Instruction(InstructionCode.LoadVar, "s"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("hello", env.Output);
    }

    [Fact]
    public void Can_define_float_variable()
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program =
        [
            new Instruction(InstructionCode.Push, new Value(3.14)),
            new Instruction(InstructionCode.DefineVar, "f"),
            new Instruction(InstructionCode.LoadVar, "f"),
            new Instruction(InstructionCode.CallBuiltin, (int)BuiltinFunctionCode.Print),
            new Instruction(InstructionCode.Push, 0),
            new Instruction(InstructionCode.Halt),
        ];

        VirtualMachine vm = new VirtualMachine(env, program);
        vm.RunProgram();

        Assert.Equal("3.14", env.Output);
    }
}