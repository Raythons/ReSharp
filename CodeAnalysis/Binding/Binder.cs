using ReSharp.CodeAnalysis;
using rs.CodeAnalysis.Syntax;

namespace rs.CodeAnalysis.Binding
{
    internal sealed class Binder
    {

        private readonly DiagonosticBag _dignostics = new DiagonosticBag();
        public DiagonosticBag Dignostics  => _dignostics;

        private readonly Dictionary<VariableSymbol, object> _variables;

        public Binder (Dictionary<VariableSymbol, object> variables)
        {

            _variables = variables;
        }

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
                    return BindParenthesizedExpression(((ParenthesizedExpressionSyntax)syntax));
                case SyntaxType.NameExpression:
                    return BindNameExpression(((NameExpressionSyntax)syntax));
                case SyntaxType.AssigmentExpression:
                    return BindAssigmentExpression(((AssigmentExpressionSyntax)syntax));


                default:
                    throw new Exception($"Unsupported syntax  {syntax.Type}");
            }
        }


        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;

            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (variable != null)
            {
                _dignostics.ReportUndefainedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);

             }
            var type = variable.Type;
            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindAssigmentExpression(AssigmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken?.Text;
            var boundExpression = BindExpression(syntax.Expression);


            var exisitingVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);
            if (exisitingVariable != null) 
                    _variables.Remove(exisitingVariable);
            
            
            var variable = new VariableSymbol(name, boundExpression.Type);
            _variables[variable] = null;
           

            return new BoundAssigmentExpression(variable, boundExpression);

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
                _dignostics.ReportUndefainedUnaryOpeartor(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);
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
                _dignostics.ReportUndefainedBinaryOpeartor(syntax.OperatorToken.Span, syntax.OperatorToken.Text, 
                    boundLeft.Type , boundRight.Type);
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }

      
    }
}


