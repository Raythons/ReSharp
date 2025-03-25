using rs.CodeAnalysis.Syntax;
namespace Resharp.Tests.Syntax;

public partial class ParserTests
{
    internal sealed class AssertingEnumerator : IDisposable
    {
        private IEnumerator<SyntaxNode> _enumerator;
        private bool _hasErrors;
        public AssertingEnumerator(SyntaxNode node)
        {
            _enumerator = Flatten(node).GetEnumerator();
        }

        private bool MarkFailed()
        {

            _hasErrors = true;
            return false;
        }
        private static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
        {
            var stack = new Stack<SyntaxNode>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();

                yield return n;

                foreach (var child in n.GetChildren().Reverse())
                    stack.Push(child);
            }
        }
        public void AssertToken(SyntaxType type, string text)
        {

            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.Equal(type, _enumerator.Current.Type);

                var token = Assert.IsType<SyntaxToken>(_enumerator.Current);

                Assert.Equal(text, token.Text);

            }
            catch when (MarkFailed())
            {
                throw;
            }
        }

        public void AssertNode(SyntaxType type)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.Equal(type, _enumerator.Current.Type);

                Assert.IsNotType<SyntaxToken>(_enumerator.Current);

            }
            catch when (MarkFailed())
            {
                throw;
            }

        }

        // public void AssertType(string text)
        // {
        //     Assert.True(_enumerator.MoveNext());
        //     Assert.IsNotType<SyntaxToken>(_enumerator.Current);
        //     Assert.Equal(type, _enumerator.Current.Type);
        // }
        public void Dispose()
        {
            if (!_hasErrors)
                Assert.False(_enumerator.MoveNext());
            _enumerator.Dispose();
        }
    }

}
