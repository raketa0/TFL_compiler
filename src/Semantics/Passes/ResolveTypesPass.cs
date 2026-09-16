using Ast.Expressions;
using Ast.Statements;

using Semantics.Exceptions;

using ValueType = Runtime.ValueType;

namespace Semantics.Passes;

/// <summary>
/// Проход по AST для вычисления типов данных.
/// </summary>
/// <exception cref="TypeErrorException">Бросается при несоответствии типов данных в процессе вычисления типов.</exception>
public sealed class ResolveTypesPass : AbstractPass
{
    public override void Visit(LiteralExpression e)
    {
        base.Visit(e);
        e.ResultType = e.Value.GetValueType();
    }

    public override void Visit(BinaryOperationExpression e)
    {
        base.Visit(e);

        ValueType? resultType = GetBinaryOperationResultType(e.Operation, e.Left.ResultType, e.Right.ResultType);
        if (resultType is null)
        {
            throw new TypeErrorException(
                $"Binary operation {e.Operation} is not allowed for types {e.Left.ResultType} and {e.Right.ResultType}"
            );
        }

        e.ResultType = resultType ?? ValueType.Void;
    }

    public override void Visit(FunctionDeclarationStatement s)
    {
        base.Visit(s);
        s.ResultType = s.ReturnType;
    }

    public override void Visit(ReturnStatement s)
    {
        base.Visit(s);
        s.ResultType = s.Expression?.ResultType ?? ValueType.Void;
    }

    public override void Visit(FunctionCallExpression e)
    {
        base.Visit(e);
        e.ResultType = e.Function.ResultType;
    }

    public override void Visit(ParametrDeclaration d)
    {
        d.ResultType = d.Type;
    }

    public override void Visit(UnaryOperationExpression e)
    {
        base.Visit(e);
        ValueType? resultType = GetUnaryOperationResultType(e.Operation, e.Operand.ResultType);
        if (resultType is null)
        {
            throw new TypeErrorException(
                $"Unary operation {e.Operation} is not allowed for type {e.Operand.ResultType}"
            );
        }

        e.ResultType = resultType ?? ValueType.Void;
    }

    public override void Visit(LengthExpression e)
    {
        base.Visit(e);

        if (e.Operand.ResultType != ValueType.String)
        {
            throw new TypeErrorException(
                $"length expects string, got {e.Operand.ResultType}"
            );
        }

        e.ResultType = ValueType.Int;
    }

    public override void Visit(SubstrExpression e)
    {
        base.Visit(e);

        if (e.Source.ResultType != ValueType.String)
        {
            throw new TypeErrorException(
                $"substr: first argument must be string, got {e.Source.ResultType}"
            );
        }

        if (e.Start.ResultType != ValueType.Int)
        {
            throw new TypeErrorException(
                $"substr: second argument must be int, got {e.Start.ResultType}"
            );
        }

        if (e.Length.ResultType != ValueType.Int)
        {
            throw new TypeErrorException(
                $"substr: third argument must be int, got {e.Length.ResultType}"
            );
        }

        e.ResultType = ValueType.String;
    }

    public override void Visit(VariableExpression e)
    {
        base.Visit(e);
        e.ResultType = e.Variable.ResultType;
    }

    public override void Visit(VariableDeclarationStatement s)
    {
        base.Visit(s);

        if (s.Value != null)
        {
            s.ResultType = s.Value.ResultType;
        }
        else
        {
            s.ResultType = s.Type;
        }
    }

    public override void Visit(ConstDeclarationStatement s)
    {
        base.Visit(s);
        s.ResultType = s.Value.ResultType;
    }

    public override void Visit(AssignmentStatement s)
    {
        s.Expression.Accept(this);
        s.ResultType = ValueType.Void;
    }

    public override void Visit(PrintStatement s)
    {
        base.Visit(s);
        s.ResultType = ValueType.Void;
    }

    public override void Visit(ReadStatement s)
    {
        if (s.Variable is ConstDeclarationStatement)
        {
            throw new TypeErrorException("Cannot read into a const variable");
        }

        Runtime.ValueType variableType = ((AbstractVariableDeclarationStatemnt)s.Variable!).ResultType;
        s.ResultType = variableType ?? Runtime.ValueType.Int;
    }

    public override void Visit(IfElseStatement s)
    {
        base.Visit(s);
        s.ResultType = ValueType.Void;
    }

    public override void Visit(WhileStatement s)
    {
        base.Visit(s);
        s.ResultType = ValueType.Void;
    }

    public override void Visit(BreakStatement s)
    {
        base.Visit(s);
        s.ResultType = ValueType.Void;
    }

    public override void Visit(ContinueStatement s)
    {
        base.Visit(s);
        s.ResultType = ValueType.Void;
    }

    private static ValueType? GetBinaryOperationResultType(BinaryOperation operaion, ValueType left, ValueType right)
    {
        switch (operaion)
        {
            case BinaryOperation.Add:
                if (left == ValueType.Int && right == ValueType.Int)
                {
                    return ValueType.Int;
                }

                if (left == ValueType.Float && right == ValueType.Float)
                {
                    return ValueType.Float;
                }

                if (left == ValueType.String && right == ValueType.String)
                {
                    return ValueType.String;
                }

                return null;
            case BinaryOperation.Subtract:
            case BinaryOperation.Multiply:
            case BinaryOperation.Divide:
            case BinaryOperation.Module:
                if (left == ValueType.Int && right == ValueType.Int)
                {
                    return ValueType.Int;
                }

                if (left == ValueType.Float && right == ValueType.Float)
                {
                    return ValueType.Float;
                }

                return null;
            case BinaryOperation.LessThan:
            case BinaryOperation.GreaterThan:
            case BinaryOperation.LessThanOrEqual:
            case BinaryOperation.GreaterThanOrEqual:
                if (left == ValueType.Float && right == ValueType.Float)
                {
                    return ValueType.Bool;
                }

                if (left == ValueType.String && right == ValueType.String)
                {
                    return ValueType.Bool;
                }

                if (left == ValueType.Int && right == ValueType.Int)
                {
                    return ValueType.Bool;
                }

                return null;
            case BinaryOperation.Equal:
            case BinaryOperation.NotEqual:
                if (left == right && left != ValueType.Void)
                {
                    return ValueType.Bool;
                }

                return null;
            case BinaryOperation.LogicalAnd:
            case BinaryOperation.LogicalOr:
                if (left == ValueType.Bool && right == ValueType.Bool)
                {
                    return ValueType.Bool;
                }

                return null;
            default:
                throw new InvalidOperationException($"Unknown binary operation {operaion}");
        }
    }

    private static ValueType? GetUnaryOperationResultType(UnaryOperation operation, ValueType valueType)
    {
        switch (operation)
        {
            case UnaryOperation.Minus:
                if (valueType == ValueType.Int)
                {
                    return ValueType.Int;
                }

                if (valueType == ValueType.Float)
                {
                    return ValueType.Float;
                }

                return null;
            case UnaryOperation.LogicalNot:
                if (valueType == ValueType.Bool)
                {
                    return ValueType.Bool;
                }

                return null;
            default:
                throw new InvalidOperationException($"Unknown binary operation {operation}");
        }
    }
}