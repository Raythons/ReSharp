using rs.CodeAnalysis.Syntax;

namespace rs.CodeAnalysis.Binding
{
    internal sealed class Binder
    {

       private readonly List<string> _dignostics = new List<string> ();
        public IEnumerable<string> Dignostics => _dignostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Type)
            {
                case SyntaxType.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxType.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                case SyntaxType.UnaryExpression:
                    return BindUnaryExpressionSyntax((UnaryExpressionSyntax)syntax);
                case SyntaxType.ParenthesizedExpression:
                    return BindExpression(((ParenthesizedExpressionSyntax)syntax).Expression);

                default:
                    throw new Exception($"Unsupported syntax  {syntax.Type}");
            }
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }
        private BoundExpression BindUnaryExpressionSyntax(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Type, boundOperand.Type);

            if(boundOperator is null)
            {
                _dignostics.Add($"Unary Operator `[{syntax.OperatorToken.Text}]` is not defined for type {boundOperand.Type}");
                return boundOperand;
            }
            return new BoundUnaryExpression(boundOperator, boundOperand);

        }


        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Type, boundLeft.Type, boundRight.Type);


            if (boundOperator == null)
            {
                _dignostics.Add($"Binary Operator `[{syntax.OperatorToken.Text}]` is not defined for type {boundLeft.Type} and {boundRight.Type}");
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }

      
    }
}


