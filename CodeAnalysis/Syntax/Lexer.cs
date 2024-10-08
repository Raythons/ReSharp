using ReSharp.CodeAnalysis;

namespace rs.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {

        private char Current => Peek(0);

        private char Lookahead => Peek(1);

        private DiagonosticBag  _diagnostics = new();
        public DiagonosticBag Diagnostics => _diagnostics;

        private readonly string _text;

        // the position in The Text
        private int _position;

        public Lexer(string text)
        {
            _text = text;
        }

        public char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
                return '\0';
            return _text[index];

        }
        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {

            if (_position >= _text.Length)
                return new SyntaxToken(SyntaxType.EndOfFileToken, _position, "\0", null);

            // Handle Numbers
            var Start = _position;

            if (char.IsDigit(Current))
            {
                // If The Number More Than 1 Digits
               while (char.IsDigit(Current))
                    Next();

                var Length = _position - Start;
                var Text = _text.Substring(Start, Length);
                if (!int.TryParse(Text, out var Value))
                    _diagnostics.ReportInvalidNumber(new TextSpan(Start, Length) , _text,typeof(int));
                return new SyntaxToken(SyntaxType.NumberToken, Start, Text, Value);
            }

            // Skipping White Spaces
            if (char.IsWhiteSpace(Current))
            {
                //var Start = _position;
                while (char.IsWhiteSpace(Current))
                    Next();
                var Length = _position - Start;
                var Text = _text.Substring(Start, Length);
                //Error Handling For Letter
                return new SyntaxToken(SyntaxType.WhiteSpaceToken, Start, Text, null);
            }

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                    Next();
                var Length = _position - Start;
                var Text = _text.Substring(Start, Length);
                //Error Handling For Letter
                return new SyntaxToken(SyntaxType.WhiteSpaceToken, Start, Text, null);
            }

            if (char.IsLetter(Current))
            {
                while (char.IsLetter(Current))
                    Next();

                var Length = _position - Start;
                var Text = _text.Substring(Start, Length);

                var type = SyntaxFacts.GetKeywordType(Text);
                return new SyntaxToken(type, Start, Text, null);
            }


            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxType.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxType.MinusToken, _position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxType.StarToken, _position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxType.SlashToken, _position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxType.OpenParenthesisToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxType.CloseParenthesisToken, _position++, ")", null);
                case '&':
                        if (Lookahead == '&')
                        {
                            _position += 2;
                             return new SyntaxToken(SyntaxType.AmperSandAmperSandToken, Start, "&&", null);
                        }   
                    break;   
                case '|':
                        if (Lookahead == '|')
                        {
                            _position += 2;
                            return new SyntaxToken(SyntaxType.PipePipeToken, Start, "||", null);
                        }                     
                    break;
                case '=':
                        if (Lookahead == '=')
                        {
                            _position += 2;
                            return new SyntaxToken(SyntaxType.EqualsEqualsToken, Start, "==", null);
                        }    
                        else
                        {
                            _position += 1;
                            return new SyntaxToken(SyntaxType.EqualsToken, Start, "=", null);
                        }
                    break;
                case '!':
                        if (Lookahead == '=')
                        {
                            _position += 2;
                            return new SyntaxToken(SyntaxType.BangEqualsToken, Start, "!=", null);
                        }
                        else {
                            _position += 1;
                            return new SyntaxToken(SyntaxType.BangToken, Start, "!", null);
                        }                   
            }

            _diagnostics.ReportBadCharacter( _position, Current);
            //  put the position then Add It - take of the vlaue from substring
            return new SyntaxToken(SyntaxType.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}


