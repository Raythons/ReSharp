namespace rs.CodeAnalysis.Syntax
{
    internal sealed class AssigmentExpressionSyntax : ExpressionSyntax
    {

        public AssigmentExpressionSyntax(SyntaxToken identifierToken, SyntaxToken equalsToken ,ExpressionSyntax expression)
        {
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Expression = expression;
        }
        public override SyntaxType Type => SyntaxType.AssigmentExpression;

        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return Expression;

        }
    }
}
