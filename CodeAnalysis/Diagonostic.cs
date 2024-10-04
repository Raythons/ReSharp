namespace ReSharp.CodeAnalysis
{
    public sealed class Diagonostic
    {
        public Diagonostic(TextSpan span, string message)
        {
            Span = span;
            Message = message;
        }

        public TextSpan Span { get; }
        public string Message { get; }
        public override string ToString() => Message;

    }
}


