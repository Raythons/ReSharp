namespace rs.CodeAnalysis.Syntax
{
    public class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxType type, int position, string text, object value)
        {
            Type = type;
            Position = position;
            Text = text;
            Value = value;
        }
        public override SyntaxType Type { get; }
        public int Position;
        public string Text;
        public object Value;


        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }

}



