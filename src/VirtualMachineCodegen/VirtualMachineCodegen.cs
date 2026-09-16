using Ast;
using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

using Runtime;

using VirtualMachine.Builtins;
using VirtualMachine.Instructions;

using ValueType = Runtime.ValueType;

namespace VirtualMachineCodegen;

public class VirtualMachineCodegen : IAstVisitor
{
    private readonly InstructionsBuilder _builder = new();
    private CodegenSymbolsTable? _symbolsTable;

    /// <summary>
    /// Стек со ссылками на блоки после текущих циклов while.
    /// Используется для генерации break.
    /// </summary>
    private readonly Stack<BasicBlock> _currentLoopFinalBlockStack = new();

    /// <summary>
    /// Стек со ссылками на блоки начала текущих циклов while.
    /// Используется для генерации continue.
    /// </summary>
    private readonly Stack<BasicBlock> _currentLoopStartBlockStack = new();

    public List<Instruction> Generate(ProgramNode program)
    {
        _symbolsTable = new CodegenSymbolsTable(null);

        BasicBlock mainBlock = _builder.InsertPoint;

        foreach (FunctionDeclarationStatement func in program.Functions)
        {
            BasicBlock functionBlock = _builder.CreateBasicBlock();
            _symbolsTable.AddFunctionEntry(func.Name, functionBlock);
        }

        _builder.InsertPoint = mainBlock;

        foreach (Statement stmt in program.Block.Statements)
        {
            if (stmt is not FunctionDeclarationStatement)
            {
                stmt.Accept(this);
            }
        }

        _builder.Append(new Instruction(InstructionCode.Push, 0));
        _builder.Append(new Instruction(InstructionCode.Halt));

        foreach (FunctionDeclarationStatement func in program.Functions)
        {
            func.Accept(this);
        }

        return _builder.Finish();
    }

    public void Visit(ProgramNode p)
    {
        p.Block.Accept(this);
    }

    public void Visit(BlockStatement s)
    {
        PushLexicalScope();

        foreach (Statement stmt in s.Statements)
        {
            if (stmt is not FunctionDeclarationStatement)
            {
                stmt.Accept(this);
            }
        }

        PopLexicalScope();
    }

    public void Visit(VariableDeclarationStatement s)
    {
        if (s.Value != null)
        {
            s.Value.Accept(this);
        }
        else
        {
            Value defaultValue = GetDefaultValue(s.Type);
            _builder.Append(new Instruction(InstructionCode.Push, defaultValue));
        }

        _builder.Append(new Instruction(InstructionCode.DefineVar, s.Name));
    }

    public void Visit(ConstDeclarationStatement s)
    {
        s.Value.Accept(this);
        _builder.Append(new Instruction(InstructionCode.DefineVar, s.Name));
    }

    public void Visit(AssignmentStatement s)
    {
        s.Expression.Accept(this);
        _builder.Append(new Instruction(InstructionCode.StoreVar, s.Name));
    }

    public void Visit(PrintStatement s)
    {
        s.Expression.Accept(this);

        _builder.Append(new Instruction(
            InstructionCode.CallBuiltin,
            (int)BuiltinFunctionCode.Print));
    }

    public void Visit(ReadStatement s)
    {
        BuiltinFunctionCode readFunction;

        if (s.ResultType is null)
        {
            throw new InvalidOperationException(
                $"Read statement for variable '{s.Name}' has no type information");
        }

        if (s.ResultType == ValueType.Int)
        {
            readFunction = BuiltinFunctionCode.ReadI;
        }
        else if (s.ResultType == ValueType.Float)
        {
            readFunction = BuiltinFunctionCode.ReadF;
        }
        else if (s.ResultType == ValueType.String)
        {
            readFunction = BuiltinFunctionCode.ReadS;
        }
        else
        {
            throw new InvalidOperationException(
                $"Unsupported type for read: {s.ResultType}");
        }

        _builder.Append(new Instruction(
            InstructionCode.CallBuiltin,
            (int)readFunction));

        _builder.Append(new Instruction(
            InstructionCode.StoreVar,
            s.Name));
    }

    public void Visit(IfElseStatement s)
    {
        if (s.ElseStatement != null)
        {
            BasicBlock elseBlock = _builder.CreateBasicBlock();
            BasicBlock finalBlock = _builder.CreateBasicBlock();

            s.Condition.Accept(this);

            _builder.AppendJump(
                InstructionCode.JumpIfFalse,
                elseBlock);

            s.Block.Accept(this);

            _builder.AppendJump(
                InstructionCode.Jump,
                finalBlock);

            _builder.InsertPoint = elseBlock;

            if (s.ElseStatement is IfElseStatement elseIf)
            {
                Visit(elseIf);
            }
            else if (s.ElseStatement is BlockStatement elseBlockStmt)
            {
                elseBlockStmt.Accept(this);
            }
            else
            {
                s.ElseStatement.Accept(this);
            }

            _builder.AppendJump(
                InstructionCode.Jump,
                finalBlock);

            _builder.InsertPoint = finalBlock;
        }
        else
        {
            BasicBlock finalBlock = _builder.CreateBasicBlock();

            s.Condition.Accept(this);

            _builder.AppendJump(
                InstructionCode.JumpIfFalse,
                finalBlock);

            s.Block.Accept(this);

            _builder.AppendJump(
                InstructionCode.Jump,
                finalBlock);

            _builder.InsertPoint = finalBlock;
        }
    }

    public void Visit(WhileStatement s)
    {
        BasicBlock loopBlock = _builder.CreateBasicBlock();
        BasicBlock finalBlock = _builder.CreateBasicBlock();

        _currentLoopStartBlockStack.Push(loopBlock);
        _currentLoopFinalBlockStack.Push(finalBlock);

        _builder.AppendJump(
            InstructionCode.Jump,
            loopBlock);

        _builder.InsertPoint = loopBlock;

        s.Expression.Accept(this);

        _builder.AppendJump(
            InstructionCode.JumpIfFalse,
            finalBlock);

        s.Block.Accept(this);

        _builder.AppendJump(
            InstructionCode.Jump,
            loopBlock);

        _currentLoopFinalBlockStack.Pop();
        _currentLoopStartBlockStack.Pop();

        _builder.InsertPoint = finalBlock;
    }

    public void Visit(BreakStatement s)
    {
        BasicBlock loopFinalBlock =
            _currentLoopFinalBlockStack.Peek();

        _builder.AppendJump(
            InstructionCode.Jump,
            loopFinalBlock);
    }

    public void Visit(ContinueStatement s)
    {
        BasicBlock loopStartBlock =
            _currentLoopStartBlockStack.Peek();

        _builder.AppendJump(
            InstructionCode.Jump,
            loopStartBlock);
    }

    public void Visit(ReturnStatement s)
    {
        if (s.Expression != null)
        {
            s.Expression.Accept(this);
        }
        else
        {
            _builder.Append(new Instruction(
                InstructionCode.Push,
                Value.Void));
        }

        _builder.Append(new Instruction(
            InstructionCode.PopVars));

        _builder.Append(new Instruction(
            InstructionCode.Return));
    }

    public void Visit(FunctionDeclarationStatement s)
    {
        BasicBlock functionBlock =
            _symbolsTable!.GetFunctionEntry(s.Name);

        BasicBlock previousBlock =
            _builder.InsertPoint;

        _builder.InsertPoint = functionBlock;

        PushLexicalScope();

        for (int i = s.Parameters.Count - 1; i >= 0; i--)
        {
            AbstractParametrStatement param =
                s.Parameters[i];

            _builder.Append(new Instruction(
                InstructionCode.DefineVar,
                param.Name));
        }

        foreach (Statement stmt in s.Body.Statements)
        {
            if (stmt is not FunctionDeclarationStatement)
            {
                stmt.Accept(this);
            }
        }

        if (s.ReturnType == ValueType.Void)
        {
            _builder.Append(new Instruction(
                InstructionCode.Push,
                Value.Void));

            _builder.Append(new Instruction(
                InstructionCode.PopVars));

            _builder.Append(new Instruction(
                InstructionCode.Return));
        }

        _builder.InsertPoint = previousBlock;
    }

    public void Visit(FunctionCallStatement s)
    {
        s.Expression.Accept(this);

        if (s.Expression.ResultType != ValueType.Void)
        {
            _builder.Append(new Instruction(
                InstructionCode.Pop));
        }
    }

    public void Visit(ParametrDeclaration s)
    {
    }

    public void Visit(LiteralExpression e)
    {
        _builder.Append(new Instruction(
            InstructionCode.Push,
            e.Value));
    }

    public void Visit(VariableExpression e)
    {
        _builder.Append(new Instruction(
            InstructionCode.LoadVar,
            e.Name));
    }

    public void Visit(BinaryOperationExpression e)
    {
        switch (e.Operation)
        {
            case BinaryOperation.Add:
                GenerateBinaryOperation(
                    e.Left,
                    e.Right,
                    InstructionCode.Add);
                break;

            case BinaryOperation.Subtract:
                GenerateBinaryOperation(
                    e.Left,
                    e.Right,
                    InstructionCode.Subtract);
                break;

            case BinaryOperation.Multiply:
                GenerateBinaryOperation(
                    e.Left,
                    e.Right,
                    InstructionCode.Multiply);
                break;

            case BinaryOperation.Divide:
                GenerateBinaryOperation(
                    e.Left,
                    e.Right,
                    InstructionCode.Divide);
                break;

            case BinaryOperation.Module:
                GenerateBinaryOperation(
                    e.Left,
                    e.Right,
                    InstructionCode.Modulo);
                break;

            case BinaryOperation.Equal:
                GenerateBinaryOperation(
                    e.Left,
                    e.Right,
                    InstructionCode.Equal);
                break;

            case BinaryOperation.NotEqual:
                GenerateBinaryOperation(
                    e.Left,
                    e.Right,
                    InstructionCode.NotEqual);
                break;

            case BinaryOperation.LessThan:
                GenerateBinaryOperation(
                    e.Left,
                    e.Right,
                    InstructionCode.Less);
                break;

            case BinaryOperation.LessThanOrEqual:
                GenerateBinaryOperation(
                    e.Left,
                    e.Right,
                    InstructionCode.LessOrEqual);
                break;

            case BinaryOperation.GreaterThan:
                GenerateBinaryOperation(
                    e.Right,
                    e.Left,
                    InstructionCode.Less);
                break;

            case BinaryOperation.GreaterThanOrEqual:
                GenerateBinaryOperation(
                    e.Right,
                    e.Left,
                    InstructionCode.LessOrEqual);
                break;

            case BinaryOperation.LogicalAnd:
                GenerateLogicalAnd(e);
                break;

            case BinaryOperation.LogicalOr:
                GenerateLogicalOr(e);
                break;

            default:
                throw new NotImplementedException(
                    $"Unsupported binary operation: {e.Operation}");
        }
    }

    public void Visit(UnaryOperationExpression e)
    {
        e.Operand.Accept(this);

        switch (e.Operation)
        {
            case UnaryOperation.Minus:
                _builder.Append(new Instruction(
                    InstructionCode.Negate));
                break;

            case UnaryOperation.LogicalNot:
                _builder.Append(new Instruction(
                    InstructionCode.Not));
                break;

            default:
                throw new NotImplementedException(
                    $"Unsupported unary operation: {e.Operation}");
        }
    }

    public void Visit(FunctionCallExpression e)
    {
        foreach (Expression arg in e.Arguments)
        {
            arg.Accept(this);
        }

        BasicBlock functionBlock =
            _symbolsTable!.GetFunctionEntry(e.Name);

        _builder.AppendJump(
            InstructionCode.Call,
            functionBlock);
    }

    public void Visit(LengthExpression e)
    {
        e.Operand.Accept(this);

        _builder.Append(new Instruction(
            InstructionCode.CallBuiltin,
            (int)BuiltinFunctionCode.Length));
    }

    public void Visit(SubstrExpression e)
    {
        e.Source.Accept(this);
        e.Start.Accept(this);
        e.Length.Accept(this);

        _builder.Append(new Instruction(
            InstructionCode.CallBuiltin,
            (int)BuiltinFunctionCode.Substr));
    }

    private void GenerateBinaryOperation(
        Expression left,
        Expression right,
        InstructionCode code)
    {
        left.Accept(this);
        right.Accept(this);

        _builder.Append(new Instruction(code));
    }

    private void GenerateLogicalAnd(
        BinaryOperationExpression e)
    {
        BasicBlock shortCircuitBlock =
            _builder.CreateBasicBlock();

        BasicBlock finalBlock =
            _builder.CreateBasicBlock();

        e.Left.Accept(this);

        _builder.AppendJump(
            InstructionCode.JumpIfFalse,
            shortCircuitBlock);

        e.Right.Accept(this);

        _builder.AppendJump(
            InstructionCode.Jump,
            finalBlock);

        _builder.InsertPoint =
            shortCircuitBlock;

        _builder.Append(new Instruction(
            InstructionCode.Push,
            new Value(false)));

        _builder.InsertPoint =
            finalBlock;
    }

    private void GenerateLogicalOr(
        BinaryOperationExpression e)
    {
        BasicBlock shortCircuitBlock =
            _builder.CreateBasicBlock();

        BasicBlock finalBlock =
            _builder.CreateBasicBlock();

        e.Left.Accept(this);

        _builder.AppendJump(
            InstructionCode.JumpIfTrue,
            shortCircuitBlock);

        e.Right.Accept(this);

        _builder.AppendJump(
            InstructionCode.Jump,
            finalBlock);

        _builder.InsertPoint =
            shortCircuitBlock;

        _builder.Append(new Instruction(
            InstructionCode.Push,
            new Value(true)));

        _builder.InsertPoint =
            finalBlock;
    }

    private static Value GetDefaultValue(
        ValueType type)
    {
        if (type == ValueType.Int)
        {
            return new Value(0);
        }
        else if (type == ValueType.Float)
        {
            return new Value(0.0);
        }
        else if (type == ValueType.String)
        {
            return new Value("");
        }
        else if (type == ValueType.Bool)
        {
            return new Value(false);
        }
        else
        {
            return new Value(0);
        }
    }

    private void PushLexicalScope()
    {
        int parentScopeDepth = _symbolsTable?.Depth ?? 0;

        _symbolsTable = new CodegenSymbolsTable(_symbolsTable);

        _builder.Append(new Instruction(
            InstructionCode.PushVars,
            parentScopeDepth));
    }

    private void PopLexicalScope()
    {
        _builder.Append(new Instruction(
            InstructionCode.PopVars));

        _symbolsTable =
            _symbolsTable!.Parent;
    }
}