namespace rs.CodeAnalysis.Syntax
{

    internal sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Right { get; }
        public ExpressionSyntax Left;
        public SyntaxToken OperatorToken;
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            OperatorToken = operatorToken;
            Left = left;
            Right = right;

        }
        public override SyntaxType Type => SyntaxType.BinaryExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}
