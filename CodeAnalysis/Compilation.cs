using rs.CodeAnalysis.Binding;
using rs.CodeAnalysis.Syntax;

namespace ReSharp.CodeAnalysis
{
    public sealed class Compilation
    {
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public SyntaxTree Syntax { get; }

        public EvaluationResult Evaluate()
        {
            var binder = new Binder();
            var boundExpression = binder.BindExpression(Syntax.Root);

            var diagonostics = Syntax.Diagnostics.Concat(binder.Dignostics).ToArray();
            if (diagonostics.Any())
                return new EvaluationResult(diagonostics, null);


            var evaluator = new Evaluator(boundExpression);
            var value = evaluator.Evaluate();

            return new EvaluationResult(Array.Empty<string>(), value);
        }

    }
}


