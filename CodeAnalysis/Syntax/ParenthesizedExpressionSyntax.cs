namespace rs.CodeAnalysis.Syntax
{
    public class ParenthesizedExpressionSyntax : ExpressionSyntax
    {

        public SyntaxToken OpenParenthesisToken;
        public ExpressionSyntax Expression;
        public SyntaxToken CloseParenthesisToken;

        public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override SyntaxType Type => SyntaxType.ParenthesizedExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return Expression;
            yield return CloseParenthesisToken;
        }
    }

}
