using rs.CodeAnalysis.Binding;
using System.Data;

namespace ReSharp.CodeAnalysis
{
    internal sealed class Evaluator
    {

        private readonly BoundExpression _root;

        public Evaluator (BoundExpression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }
        //      +
        //    /   \
        //    1   *   (expression)
        //      /  \
        //     2    3
        //
        private object EvaluateExpression(BoundExpression node)
        {
            // We Have Two Now
            if (node is BoundLiteralExpression n)
                return n.Value;

            if (node is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.Op.OperatorType)
                {
                    case BoundUnaryOperatorType.Negation:
                        return -(int)operand;
                    case BoundUnaryOperatorType.Identity:
                        return (int)operand;
                    case BoundUnaryOperatorType.LogicalNegation:
                        return !(bool)operand;
                    default:
                        throw new Exception($"Unexpected Unary Operator {u.Op.OperatorType} ");
                }
            }


            if (node is BoundBinaryExpression b)
            {
                var Left = EvaluateExpression(b.Left);
                var Right = EvaluateExpression(b.Right);

                switch (b.Op.OperatorType)
                {
                    case BoundBinaryOperatorType.Addition:
                        return (int)Left + (int)Right;
                    case BoundBinaryOperatorType.Subtraction:
                        return (int)Left - (int)Right;
                    case BoundBinaryOperatorType.Multiplication:
                        return (int)Left * (int)Right;
                    case BoundBinaryOperatorType.Division:
                        return (int)Left / (int)Right;
                    case BoundBinaryOperatorType.LogicalAnd:
                        return (bool)Left && (bool)Right;
                    case BoundBinaryOperatorType.LogicalOr:
                        return (bool)Left || (bool)Right;
                    case BoundBinaryOperatorType.Equals:
                        return Equals(Left,Right);
                    case BoundBinaryOperatorType.NotEquals:
                        return !Equals(Left, Right);
                    default:
                        throw new Exception($"Unexpected Binary Operator {b.Op.OperatorType} ");
                }
            }

            //if (node is ParenthesizedExpressionSyntax p)
            //    return EvaluateExpression(p.Expression);

            throw new Exception($"Unexpected node {node.Type} ");


        }


    }
}


