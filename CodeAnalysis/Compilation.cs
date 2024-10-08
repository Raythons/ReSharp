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

        public EvaluationResult Evaluate(Dictionary<string, object> variables)
        {
            var binder = new Binder(variables);
            var boundExpression = binder.BindExpression(Syntax.Root);

            var diagonostics = Syntax.Diagnostics.Concat(binder.Dignostics).ToArray();
            if (diagonostics.Any())
                return new EvaluationResult(diagonostics, null);


            var evaluator = new Evaluator(boundExpression, variables);
            var value = evaluator.Evaluate();

            return new EvaluationResult(Array.Empty<Diagonostic>(), value);
        }
    }
}


