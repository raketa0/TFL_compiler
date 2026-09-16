using Runtime;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

namespace VirtualMachine;

public class VirtualMachine
{
    private readonly BuiltinFunctions _builtinFunctions;
    private readonly IReadOnlyList<Instruction> _instructions;
    private int _instructionPointer;
    private int _exitCode;
    private readonly Stack<Value> _evaluationStack;
    private VariablesTable? _variables;
    private readonly Stack<ReturnContext> _returnStack;
    private Value _result;

    public VirtualMachine(IEnvironment environment, IReadOnlyList<Instruction> instructions)
    {
        ValidateInstructions(instructions);

        _builtinFunctions = new BuiltinFunctions(environment);
        _instructions = instructions;

        _instructionPointer = 0;
        _exitCode = 0;

        _evaluationStack = new Stack<Value>();
        _variables = new VariablesTable();
        _returnStack = [];
        _result = Value.Void;
    }

    public int ExitCode => _exitCode;

    public int RunProgram()
    {
        while (true)
        {
            Instruction instruction = _instructions[_instructionPointer++];

            switch (instruction.Code)
            {
                case InstructionCode.Push:
                    _evaluationStack.Push(instruction.Operand);
                    break;

                case InstructionCode.Pop:
                    _evaluationStack.Pop();
                    break;

                case InstructionCode.DefineVar:
                    {
                        Value value = _evaluationStack.Pop();
                        string name = instruction.Operand.AsString();
                        _variables!.DefineVariable(name, value);
                        break;
                    }

                case InstructionCode.StoreVar:
                    {
                        Value value = _evaluationStack.Pop();
                        string name = instruction.Operand.AsString();
                        _variables!.AssignVariable(name, value);
                        break;
                    }

                case InstructionCode.LoadVar:
                    {
                        string name = instruction.Operand.AsString();
                        Value value = _variables!.GetVariable(name);
                        _evaluationStack.Push(value);
                        break;
                    }

                case InstructionCode.Add:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsString())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsString() + right.AsString())
                            );
                        }
                        else if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() + right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() + right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Subtract:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() - right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() - right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Multiply:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() * right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() * right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Divide:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() / right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() / right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Modulo:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsFloat())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsFloat() % right.AsFloat())
                            );
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() % right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.And:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsBool())
                        {
                            _evaluationStack.Push(new Value(left.AsBool() && right.AsBool()));
                        }
                        else
                        {
                            if (left.AsInt() != 0 && right.AsInt() != 0)
                            {
                                _evaluationStack.Push(new Value(1));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(0));
                            }
                        }

                        break;
                    }

                case InstructionCode.Or:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsBool())
                        {
                            _evaluationStack.Push(new Value(left.AsBool() || right.AsBool()));
                        }
                        else
                        {
                            if (left.AsInt() != 0 || right.AsInt() != 0)
                            {
                                _evaluationStack.Push(new Value(1));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(0));
                            }
                        }

                        break;
                    }

                case InstructionCode.Not:
                    {
                        Value value = _evaluationStack.Pop();

                        if (value.IsBool())
                        {
                            _evaluationStack.Push(new Value(!value.AsBool()));
                        }
                        else
                        {
                            if (value.AsInt() == 0)
                            {
                                _evaluationStack.Push(new Value(1));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(0));
                            }
                        }

                        break;
                    }

                case InstructionCode.Negate:
                    {
                        Value value = _evaluationStack.Pop();

                        if (value.IsFloat())
                        {
                            _evaluationStack.Push(new Value(-value.AsFloat()));
                        }
                        else
                        {
                            _evaluationStack.Push(new Value(-value.AsInt()));
                        }

                        break;
                    }

                case InstructionCode.Equal:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsBool())
                        {
                            _evaluationStack.Push(new Value(left.AsBool() == right.AsBool()));
                        }
                        else if (left.IsString())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsString() == right.AsString())
                            );
                        }
                        else if (left.IsFloat())
                        {
                            if (Math.Abs(left.AsFloat() - right.AsFloat()) < double.Epsilon)
                            {
                                _evaluationStack.Push(new Value(true));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(false));
                            }
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() == right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.NotEqual:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsBool())
                        {
                            _evaluationStack.Push(new Value(left.AsBool() != right.AsBool()));
                        }
                        else if (left.IsString())
                        {
                            _evaluationStack.Push(
                                new Value(left.AsString() != right.AsString())
                            );
                        }
                        else if (left.IsFloat())
                        {
                            if (Math.Abs(left.AsFloat() - right.AsFloat()) >= double.Epsilon)
                            {
                                _evaluationStack.Push(new Value(true));
                            }
                            else
                            {
                                _evaluationStack.Push(new Value(false));
                            }
                        }
                        else
                        {
                            _evaluationStack.Push(
                                new Value(left.AsInt() != right.AsInt())
                            );
                        }

                        break;
                    }

                case InstructionCode.Less:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsString())
                        {
                            int cmp = string.Compare(
                                left.AsString(), right.AsString(), StringComparison.Ordinal
                            );
                            _evaluationStack.Push(new Value(cmp < 0));
                        }
                        else if (left.IsFloat())
                        {
                            _evaluationStack.Push(new Value(left.AsFloat() < right.AsFloat()));
                        }
                        else
                        {
                            _evaluationStack.Push(new Value(left.AsInt() < right.AsInt()));
                        }

                        break;
                    }

                case InstructionCode.LessOrEqual:
                    {
                        Value right = _evaluationStack.Pop();
                        Value left = _evaluationStack.Pop();

                        if (left.IsString())
                        {
                            int cmp = string.Compare(
                                left.AsString(), right.AsString(), StringComparison.Ordinal
                            );
                            _evaluationStack.Push(new Value(cmp <= 0));
                        }
                        else if (left.IsFloat())
                        {
                            _evaluationStack.Push(new Value(left.AsFloat() <= right.AsFloat()));
                        }
                        else
                        {
                            _evaluationStack.Push(new Value(left.AsInt() <= right.AsInt()));
                        }

                        break;
                    }

                case InstructionCode.Jump:
                    {
                        _instructionPointer = instruction.Operand.AsInt();
                        break;
                    }

                case InstructionCode.JumpIfTrue:
                    {
                        Value condition = _evaluationStack.Pop();
                        bool isTrue;

                        if (condition.IsBool())
                        {
                            isTrue = condition.AsBool();
                        }
                        else
                        {
                            isTrue = condition.AsInt() != 0;
                        }

                        if (isTrue)
                        {
                            _instructionPointer = instruction.Operand.AsInt();
                        }

                        break;
                    }

                case InstructionCode.JumpIfFalse:
                    {
                        Value condition = _evaluationStack.Pop();
                        bool isFalse;

                        if (condition.IsBool())
                        {
                            isFalse = !condition.AsBool();
                        }
                        else
                        {
                            isFalse = condition.AsInt() == 0;
                        }

                        if (isFalse)
                        {
                            _instructionPointer = instruction.Operand.AsInt();
                        }

                        break;
                    }

                case InstructionCode.CallBuiltin:
                    {
                        CallBuiltin((BuiltinFunctionCode)instruction.Operand.AsInt());
                        break;
                    }

                case InstructionCode.Call:
                    {
                        _returnStack.Push(new ReturnContext(
                            _instructionPointer,
                            _variables
                        ));
                        _instructionPointer = instruction.Operand.AsInt();
                        break;
                    }

                case InstructionCode.Return:
                    {
                        ReturnContext context = _returnStack.Pop();
                        _instructionPointer = context.InstructionPointer;
                        _variables = context.Variables;
                        break;
                    }

                case InstructionCode.StoreResult:
                    {
                        _result = _evaluationStack.Pop();
                        break;
                    }

                case InstructionCode.PushVars:
                    {
                        _variables = new VariablesTable(_variables);
                        break;
                    }

                case InstructionCode.PopVars:
                    {
                        _variables = _variables!.Parent
                            ?? throw new InvalidOperationException("Variables table stack underflow");
                        break;
                    }

                case InstructionCode.Halt:
                    {
                        _exitCode = _evaluationStack.Pop().AsInt();
                        return _exitCode;
                    }

                default:
                    throw new NotImplementedException(instruction.Code.ToString());
            }
        }
    }

    /// <summary>
    /// Выполняет вызов встроенной функции.
    /// </summary>
    private void CallBuiltin(BuiltinFunctionCode code)
    {
        switch (code)
        {
            case BuiltinFunctionCode.Print:
                _builtinFunctions.Print(_evaluationStack.Pop());
                break;

            case BuiltinFunctionCode.ReadI:
                _evaluationStack.Push(_builtinFunctions.ReadI());
                break;

            case BuiltinFunctionCode.ReadF:
                _evaluationStack.Push(_builtinFunctions.ReadF());
                break;

            case BuiltinFunctionCode.ReadS:
                _evaluationStack.Push(_builtinFunctions.ReadS());
                break;

            case BuiltinFunctionCode.Length:
                {
                    Value arg = _evaluationStack.Pop();
                    _evaluationStack.Push(_builtinFunctions.Length(arg));
                    break;
                }

            case BuiltinFunctionCode.Substr:
                {
                    Value substrLength = _evaluationStack.Pop();
                    Value substrStart = _evaluationStack.Pop();
                    Value substrSource = _evaluationStack.Pop();

                    _evaluationStack.Push(
                        _builtinFunctions.Substr(substrSource, substrStart, substrLength)
                    );

                    break;
                }

            default:
                throw new InvalidOperationException($"Unknown builtin {code}");
        }
    }

    private static void ValidateInstructions(IReadOnlyList<Instruction> instructions)
    {
        if (instructions.Count == 0)
        {
            throw new InvalidOperationException("Program is empty");
        }

        InstructionCode lastInstructionCode = instructions[^1].Code;

        if (lastInstructionCode != InstructionCode.Halt
            && lastInstructionCode != InstructionCode.Return
            && lastInstructionCode != InstructionCode.Jump)
        {
            throw new InvalidOperationException(
                $"Last instruction must be {InstructionCode.Halt}," +
                $" {InstructionCode.Return} or {InstructionCode.Jump}, got {lastInstructionCode}"
            );
        }
    }

    private record struct ReturnContext(
        int InstructionPointer,
        VariablesTable? Variables
    );
}