using VirtualMachine;

namespace Interpreter;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: Interpreter <file-path>");
            return 1;
        }

        string sourcePath = args[0];

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Error: file '{sourcePath}' not found.");
            return 1;
        }

        try
        {
            string code = File.ReadAllText(sourcePath);

            ConsoleEnvironment environment = new ConsoleEnvironment();
            Interpreter interpreter = new Interpreter(environment);

            interpreter.Execute(code);

            return interpreter.ExitCode;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}