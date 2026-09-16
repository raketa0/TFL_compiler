using VirtualMachine.Instructions;

namespace VirtualMachineCodegen;

public class InstructionsBuilder
{
    private readonly List<BasicBlock> _basicBlocks;
    private BasicBlock _insertPoint;

    public InstructionsBuilder()
    {
        _basicBlocks = new List<BasicBlock>();
        _insertPoint = CreateBasicBlock();
    }

    public BasicBlock InsertPoint
    {
        get => _insertPoint;
        set
        {
            if (!ReferenceEquals(_basicBlocks[value.Id], value))
            {
                throw new InvalidOperationException(
                    "Basic block does not belong to the current instructions builder");
            }

            _insertPoint = value;
        }
    }

    /// <summary>
    /// Добавляет инструкцию в текущий базовый блок.
    /// Инструкции перехода добавляются другим методом.
    /// </summary>
    public void Append(Instruction instruction)
    {
        if (IsJump(instruction.Code))
        {
            throw new InvalidOperationException(
                $"Cannot append {instruction.Code} using this method");
        }

        _insertPoint.Append(instruction);
    }

    /// <summary>
    /// Добавляет инструкцию перехода на указанный базовый блок.
    /// </summary>
    public void AppendJump(InstructionCode code, BasicBlock target)
    {
        if (!IsJump(code))
        {
            throw new InvalidOperationException(
                $"Instruction {code} is not a jump instruction");
        }

        _insertPoint.Append(new Instruction(code, target.Id));
    }

    /// <summary>
    /// Создаёт базовый блок инструкций и возвращает ссылку на него.
    /// </summary>
    public BasicBlock CreateBasicBlock()
    {
        BasicBlock bb = new BasicBlock(_basicBlocks.Count);
        _basicBlocks.Add(bb);
        return bb;
    }

    /// <summary>
    /// Собирает финальный список инструкций из базовых блоков, заменяя адреса во всех инструкциях перехода
    /// на окончательные адреса инструкций.
    /// </summary>
    public List<Instruction> Finish()
    {
        List<int> addresses = CalculateBasicBlockAddresses();
        List<Instruction> instructions = new List<Instruction>();

        foreach (BasicBlock block in _basicBlocks)
        {
            foreach (Instruction instruction in block.Instructions)
            {
                if (IsJump(instruction.Code))
                {
                    int newAddress = addresses[instruction.Operand.AsInt()];
                    instructions.Add(new Instruction(instruction.Code, newAddress));
                }
                else
                {
                    instructions.Add(instruction);
                }
            }
        }

        return instructions;
    }

    private bool IsJump(InstructionCode code)
    {
        return code switch
        {
            InstructionCode.Call => true,
            InstructionCode.Jump => true,
            InstructionCode.JumpIfFalse => true,
            InstructionCode.JumpIfTrue => true,
            _ => false,
        };
    }

    private List<int> CalculateBasicBlockAddresses()
    {
        List<int> basicBlockAddresses = new List<int>(_basicBlocks.Count);
        int nextBlockAddress = 0;

        foreach (BasicBlock bb in _basicBlocks)
        {
            basicBlockAddresses.Add(nextBlockAddress);
            nextBlockAddress += bb.Instructions.Count;
        }

        return basicBlockAddresses;
    }
}