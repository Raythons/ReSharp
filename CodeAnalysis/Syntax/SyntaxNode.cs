namespace rs.CodeAnalysis.Syntax
{

    public abstract class SyntaxNode
    {
        public abstract SyntaxType Type { get; }

        abstract public IEnumerable<SyntaxNode> GetChildren();
    }
}