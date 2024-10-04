namespace ReSharp.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(IEnumerable<Diagonostic> diagonostics, object value)
        {
            Diagonostics = diagonostics.ToArray();
            Value = value;
        }

        public IReadOnlyList<Diagonostic> Diagonostics { get; }
        public object Value { get; }
    }
}


