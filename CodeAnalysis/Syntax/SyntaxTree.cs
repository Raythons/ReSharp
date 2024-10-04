namespace rs.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public IReadOnlyList<string> Diagnostics { get; }
        public readonly ExpressionSyntax Root;
        public readonly SyntaxToken EndOfFileToken;
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            EndOfFileToken = endOfFileToken;
            Root = root;
            Diagnostics = diagnostics.ToArray();
        }

        public static SyntaxTree Parse(string text)
        {
            var Parser = new Parser(text);
            return Parser.Parse();
        }
    }

}
