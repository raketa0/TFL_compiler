using Ast.Expressions;
using Ast.Program;
using Ast.Statements;

using Semantics.Exceptions;
using Semantics.Helpers;

using ValueType = Runtime.ValueType;

namespace Semantics.Passes;

/// <summary>
/// Проход по AST для проверки корректности программы с точки зрения совместимости типов данных.
/// </summary>
/// <exception cref="TypeErrorException">Бросается при несоответствии типов данных в процессе проверки.</exception>
public class CheckTypesPass : AbstractPass
{
    private FunctionDeclarationStatement? _currentFunction;
    private bool _allowVoidExpression = false;

    public override void Visit(BinaryOperationExpression e)
    {
        base.Visit(e);

        if (e.ResultType is null)
        {
            throw new TypeErrorException(
                $"Binary operation {e.Operation} is not allowed for types {e.Left.ResultType} and {e.Right.ResultType}"
            );
        }
    }

    public override void Visit(UnaryOperationExpression e)
    {
        base.Visit(e);

        if (e.Operation == UnaryOperation.Minus)
        {
            if (e.Operand.ResultType != ValueType.Int &&
                e.Operand.ResultType != ValueType.Float)
            {
                throw new TypeErrorException("Unary minus allowed only for int or float");
            }
        }
        else if (e.Operation == UnaryOperation.LogicalNot)
        {
            if (e.Operand.ResultType != ValueType.Bool)
            {
                throw new TypeErrorException("Logical not allowed only for bool");
            }
        }
    }

    public override void Visit(FunctionCallStatement s)
    {
        _allowVoidExpression = true;
        base.Visit(s);
        _allowVoidExpression = false;
    }

    public override void Visit(FunctionCallExpression e)
    {
        bool allowed = _allowVoidExpression;
        _allowVoidExpression = false;
        base.Visit(e);
        CheckFunctionArgumentTypes(e, e.Function);

        if (!allowed && e.ResultType == ValueType.Void)
        {
            throw new TypeErrorException($"Cannot use void function '{e.Name}' as a value expression");
        }
    }

    public override void Visit(FunctionDeclarationStatement s)
    {
        FunctionDeclarationStatement? previousFunction = _currentFunction;
        _currentFunction = s;
        try
        {
            base.Visit(s);
        }
        finally
        {
            _currentFunction = previousFunction;
        }
    }

    public override void Visit(ReturnStatement s)
    {
        base.Visit(s);

        if (_currentFunction == null)
        {
            throw new InvalidReturnStatementException("return statement is not allowed outside a function");
        }

        ValueType expectedType = _currentFunction.ReturnType;

        if (s.Expression != null)
        {
            if (expectedType == ValueType.Void)
            {
                throw new TypeErrorException(
                    $"Cannot return a value from void function '{_currentFunction.Name}'");
            }

            CheckAreSameTypes("return statement", s.Expression, expectedType);
        }
        else
        {
            if (expectedType != ValueType.Void)
            {
                throw new TypeErrorException(
                    $"return without value in non-void function '{_currentFunction.Name}'");
            }
        }
    }

    public override void Visit(VariableDeclarationStatement s)
    {
        base.Visit(s);

        if (s.Value != null)
        {
            CheckAreSameTypes("variable initialization", s.Value, s.Type);
        }
    }

    public override void Visit(ConstDeclarationStatement s)
    {
        base.Visit(s);
        CheckAreSameTypes("const initialization", s.Value, s.Type);
    }

    public override void Visit(AssignmentStatement s)
    {
        base.Visit(s);
        CheckAreSameTypes("assignment", s.Expression, s.Variable.ResultType);
    }

    public override void Visit(IfElseStatement s)
    {
        base.Visit(s);

        CheckAreSameTypes("if-else condition", s.Condition, ValueType.Bool);
    }

    public override void Visit(WhileStatement s)
    {
        base.Visit(s);

        CheckAreSameTypes("while loop condition", s.Expression, ValueType.Bool);
    }

    private static void CheckAreSameTypes(string category, Expression expression, ValueType expectedType)
    {
        if (!ValueTypeUtil.AreCompatibleTypes(expression.ResultType, expectedType))
        {
            throw new TypeErrorException(category, expectedType, expression.ResultType);
        }
    }

    private static void CheckFunctionArgumentTypes(
        FunctionCallExpression e,
        AbstractFunctionDeclarationStatement function)
    {
        if (e.Arguments.Count != function.Parameters.Count)
        {
            throw new TypeErrorException(
                $"Function '{e.Name}' expects {function.Parameters.Count} argument(s), but got {e.Arguments.Count}");
        }

        for (int i = 0, iMax = e.Arguments.Count; i < iMax; i++)
        {
            Expression argument = e.Arguments[i];
            AbstractParametrStatement param = function.Parameters[i];
            if (!ValueTypeUtil.AreCompatibleTypes(argument.ResultType, param.ResultType))
            {
                throw new TypeErrorException(
                    $"Cannot apply argument #{i} of type {argument.ResultType} to function {e.Name} parameter {param.Name} which has type {param.ResultType}"
                );
            }
        }
    }
}