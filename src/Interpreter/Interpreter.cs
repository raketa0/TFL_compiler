using Ast.Program;
using Parsing;
using Runtime;
using Semantics;
using VirtualMachine;
using VirtualMachine.Instructions;

namespace Interpreter;

public class Interpreter
{
    private readonly IEnvironment _environment;
    private int _exitCode;

    public Interpreter(IEnvironment environment)
    {
        _environment = environment;
    }

    public int ExitCode => _exitCode;

    public Value Execute(string code)
    {
        Parser parser = new Parser(code);
        ProgramNode program = parser.ParseProgram();

        SemanticsChecker checker = new SemanticsChecker();
        checker.Check(program);

        VirtualMachineCodegen.VirtualMachineCodegen codegen = new VirtualMachineCodegen.VirtualMachineCodegen();
        List<Instruction> instructions = codegen.Generate(program);

        VirtualMachine.VirtualMachine vm = new VirtualMachine.VirtualMachine(_environment, instructions);
        int result = vm.RunProgram();

        _exitCode = vm.ExitCode;

        return new Value(result);
    }
}