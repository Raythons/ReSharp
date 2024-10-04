namespace rs.CodeAnalysis.Syntax
{
    internal sealed class UnaryExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Operand { get; }
        public SyntaxToken OperatorToken;

        public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax operand)
        {
            Operand = operand;
            OperatorToken = operatorToken;
        }

        public override SyntaxType Type => SyntaxType.UnaryExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }
}
