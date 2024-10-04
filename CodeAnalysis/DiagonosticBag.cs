using rs.CodeAnalysis.Syntax;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace ReSharp.CodeAnalysis
{
    internal sealed class DiagonosticBag : IEnumerable<Diagonostic> 
    {
        private readonly List<Diagonostic> _diagonostics = new List<Diagonostic>();
        public IEnumerator<Diagonostic> GetEnumerator() => _diagonostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Report(TextSpan span, string Message)
        {
            var diagonostic = new Diagonostic(span, Message);
            _diagonostics.Add(diagonostic);
        }
        public void AddRange(DiagonosticBag diagnostics)
        {
            _diagonostics.AddRange(diagnostics);
        }

        public void ReportBadCharacter(int position, char character)
        {
            var message = $"Bad  character  input {character}";
            var span = new TextSpan(position, 1);
            Report(span , message);
        }

        public void ReportInvalidNumber(TextSpan span, string text, Type type)
        {
            var message = $"The number {text} is Valid ${type}";
            Report(span, message);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxType actualType, SyntaxType expectedType)
        {
            var message = $"Unexpected Token <{actualType}>, expected <{expectedType}>";
            Report(span, message);

        }

        public void ReportUndefainedUnaryOpeartor(TextSpan span, string operatorText, Type operandType)
        {
            var message = $"Unary Operator `[{operatorText}]` is not defined for type {operandType}";
            Report(span, message);
        }

        internal void ReportUndefainedBinaryOpeartor(TextSpan span, string operatorText, Type leftType, Type rightType)
        {
            var message = $"Binary Operator `[{operatorText}]` is not defined for type {leftType} and {rightType}";
            Report(span, message);
        }
    }
}


