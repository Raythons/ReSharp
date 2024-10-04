namespace rs.CodeAnalysis.Syntax
{
    sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        public SyntaxToken LiteralToken { get; }
        public object Value { get; }


        public LiteralExpressionSyntax(SyntaxToken literalToken) :
            this(literalToken, literalToken.Value)
        {
        }

        public LiteralExpressionSyntax(SyntaxToken literalToken, object value)
        {
            LiteralToken = literalToken;
            Value = value;
        }
        public override SyntaxType Type => SyntaxType.LiteralExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}





