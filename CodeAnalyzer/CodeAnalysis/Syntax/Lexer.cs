using ReSharp.CodeAnalysis;

namespace rs.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {

        private char Current => Peek(0);

        private readonly DiagonosticBag _diagnostics = new();
        private int _position;
        private int _start;
        private SyntaxType _type;
        private object _value;

        private char Lookahead => Peek(1);

        public DiagonosticBag Diagnostics => _diagnostics;

        private readonly string _text;

        // the position in The Text

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


        public SyntaxToken Lex()
        {

            // Handle Numbers
            _start = _position;
            _type = SyntaxType.BadToken;
            _value = null;

            switch (Current)
            {
                case '\0':
                    _type = SyntaxType.EndOfFileToken;
                    break;

                case '+':
                    _type = SyntaxType.PlusToken;
                    _position++;
                    break;
                case '-':
                    _type = SyntaxType.MinusToken;
                    _position++;
                    break;
                case '*':
                    _type = SyntaxType.StarToken;
                    _position++;
                    break;
                case '/':
                    _type = SyntaxType.SlashToken;
                    _position++;
                    break;
                case '(':
                    _type = SyntaxType.OpenParenthesisToken;
                    _position++;
                    break;
                case ')':
                    _type = SyntaxType.CloseParenthesisToken;
                    _position++;
                    break;
                case '&':
                    if (Lookahead == '&')
                    {
                        _type = SyntaxType.AmperSandAmperSandToken;
                        _position += 2;
                        break;
                    }
                    _position++;
                    break;
                case '|':
                    if (Lookahead == '|')
                    {
                        _type = SyntaxType.PipePipeToken;
                        _position += 2;
                        break;
                    }
                    _position++;
                    break;
                case '=':
                    _position++;
                    if (Current != '=')
                    {
                        _type = SyntaxType.EqualsToken;
                        _position += 1;
                    }
                    else
                    {
                        _type = SyntaxType.EqualsEqualsToken;
                    }
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ReadNumberToken();
                    break;
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    ReadWhiteSpace();
                    break;
                case '!':
                    _position++;
                    if (Current != '=')
                    {
                        _type = SyntaxType.BangToken;
                    }
                    else
                    {
                        _type = SyntaxType.BangEqualsToken;
                        _position++;
                    }
                    break;
                default:
                    if (char.IsLetter(Current))
                        ReadIdentifierOrKeyWord();
                    else if (char.IsWhiteSpace(Current))
                        ReadWhiteSpace();
                    else
                    {
                        _diagnostics.ReportBadCharacter(_position, Current);
                        _position++;
                        break;
                    }
                    break;
            }
            var length = _position - _start;
            var text = SyntaxFacts.GetText(_type);
            if (text == null)
                text = _text.Substring(_start, length);
            //  put the position then Add It - take of the vlaue from substring
            return new SyntaxToken(_type, _start, text, _value);
        }




        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
                _position++;
            _type = SyntaxType.WhiteSpaceToken;
        }

        private void ReadIdentifierOrKeyWord()
        {
            while (char.IsLetter(Current))
                _position++;

            var Length = _position - _start;
            var Text = _text.Substring(_start, Length);

            _type = SyntaxFacts.GetKeywordType(Text);
        }



        private void ReadNumberToken()
        {
            while (char.IsDigit(Current))
                _position++;
            var Length = _position - _start;
            var Text = _text.Substring(_start, Length);
            if (!int.TryParse(Text, out var Value))
                _diagnostics.ReportInvalidNumber(new TextSpan(_start, Length), _text, typeof(int));

            _value = Value;
            _type = SyntaxType.NumberToken;
        }
    }
}



// // If The Number More Than 1 Digits
// while (char.IsDigit(Current))
//     Next();
// var text = SyntaxFacts.GetText();