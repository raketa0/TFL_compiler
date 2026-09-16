using Tests.TestLibrary.TestDoubles;

using VirtualMachine.Instructions;

namespace VirtualMachine.UnitTests;

public class HaltTest
{
    [Theory]
    [MemberData(nameof(GetHaltVmData))]
    public void Can_halt_VM(int exitCode)
    {
        FakeEnvironment env = new FakeEnvironment();

        List<Instruction> program = new List<Instruction>
        {
            new Instruction(InstructionCode.Push, exitCode),
            new Instruction(InstructionCode.Halt),
        };

        VirtualMachine vm = new VirtualMachine(env, program);

        vm.RunProgram();

        Assert.Equal(exitCode, vm.ExitCode);
        Assert.Equal(string.Empty, env.Output);
    }

    public static TheoryData<int> GetHaltVmData()
    {
        return new TheoryData<int>
        {
            0,   // Остановка виртуальной машины с нулевым кодом
            1,   // Остановка виртуальной машины с ненулевым кодом
        };
    }
}