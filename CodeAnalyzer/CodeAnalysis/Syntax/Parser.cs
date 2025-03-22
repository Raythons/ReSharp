using rs.CodeAnalysis;
using ReSharp.CodeAnalysis;
using System.Runtime.Intrinsics.X86;

namespace rs.CodeAnalysis.Syntax
{
    internal sealed partial class Parser
    {
        private SyntaxToken[] _tokens;
        private int _position;
        private DiagonosticBag _diagnostics = new();
        public DiagonosticBag Diagnostics => _diagnostics;

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

            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Type, type);
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
        public ExpressionSyntax ParseExpression()
        {
            return ParseAssigmentExpression();
        }


        private ExpressionSyntax ParseAssigmentExpression()
        {

            if(Peek(0).Type  == SyntaxType.IdentifierToken  &&
                Peek(1).Type == SyntaxType.EqualsToken)
            {
                var idenfifierToken = NextToken();
                var operatorToken = NextToken();
                var right = ParseAssigmentExpression();
                return new AssigmentExpressionSyntax(idenfifierToken, operatorToken, right);

            }

            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var opeartorToken = NextToken();
                var operand = ParseBinaryExpression(unaryOperatorPrecedence);
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
                var right = ParseBinaryExpression(precedence);
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
                    var Expression = ParseBinaryExpression();
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

                case SyntaxType.IdentifierToken:
                    {
                        var identifierToken = NextToken();
                        return new NameExpressionSyntax(identifierToken);
                    }
            }
            var NumberSyntaxToken = MatchToken(SyntaxType.NumberToken);
            return new LiteralExpressionSyntax(NumberSyntaxToken);
        }
    }
}
