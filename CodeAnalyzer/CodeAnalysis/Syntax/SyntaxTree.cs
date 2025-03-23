using ReSharp.CodeAnalysis;

namespace rs.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public IReadOnlyList<Diagonostic> Diagnostics { get; }
        public readonly ExpressionSyntax Root;
        public readonly SyntaxToken EndOfFileToken;
        public SyntaxTree(IEnumerable<Diagonostic> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
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

        public static IEnumerable<SyntaxToken> ParseTokens(string text)
        {
            var Lexer = new Lexer(text);
            while (true)
            {
                var token = Lexer.Lex();

                if (token.Type == SyntaxType.EndOfFileToken)
                    break;

                yield return token;
            }
        }
    }

}
