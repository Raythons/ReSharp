using rs.CodeAnalysis;
using ReSharp.CodeAnalysis;
using System.Runtime.Intrinsics.X86;

namespace rs.CodeAnalysis.Syntax
{
    public sealed partial class Parser
    {
        private SyntaxToken[] _tokens;
        private int _position;
        private List<string> _diagnostics = new();
        public IEnumerable<string> Diagnostics => _diagnostics;

        public Parser(string text)
        {
            var Tokens = new List<SyntaxToken>();
            var lexer = new Lexer(text);
            SyntaxToken Token;

            do
            {
                Token = lexer.Lex();
                if (
                    Token.Type != SyntaxType.WhiteSpaceToken &&
                    Token.Type != SyntaxType.BadToken
                    )
                {
                    Tokens.Add(Token);
                }

            } while (Token.Type != SyntaxType.EndOfFileToken);

            _tokens = Tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];
            return _tokens[index];
        }
        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }
        private SyntaxToken MatchToken(SyntaxType type)
        {
            if (Current.Type == type)
                return NextToken();

            _diagnostics.Add($"ERROR: Unexpected Token <{Current.Type}>, expected <{type}>");
            return new SyntaxToken(type, Current.Position, null, null);
        }

        private SyntaxToken Current => Peek(0);

        // this is An Op Code
        public SyntaxTree Parse()
        {
            var Expression = ParseExpression();
            var EndOfFileToken = MatchToken(SyntaxType.EndOfFileToken);
            return new SyntaxTree(_diagnostics, Expression, EndOfFileToken);
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var opeartorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(opeartorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }


            while (true)
            {
                var precedence = Current.Type.GetBinaryOperatorPrecedence();

                if (precedence == 0 || precedence <= parentPrecedence)
                    break;
                var operationToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operationToken, right);
            }
            return left;
        }


        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Type)
            {
                case SyntaxType.OpenParenthesisToken:
                {
                    var Left = NextToken();
                    var Expression = ParseExpression();
                    var Right = MatchToken(SyntaxType.CloseParenthesisToken);
                    return new ParenthesizedExpressionSyntax(Left, Expression, Right);
                }

                case SyntaxType.TrueKeyword:
                case SyntaxType.FalseKeyword:
                {
                    var keywordToken = NextToken();
                    var value = keywordToken.Type == SyntaxType.TrueKeyword;
                    return new LiteralExpressionSyntax(keywordToken, value);
                }
            }
            var NumberSyntaxToken = MatchToken(SyntaxType.NumberToken);
            return new LiteralExpressionSyntax(NumberSyntaxToken);
        }
    }
}
