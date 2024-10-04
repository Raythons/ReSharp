namespace ReSharp.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(IReadOnlyList<string> diagonostics, object value)
        {
            Diagonostics = diagonostics;
            Value = value;
        }

        public IReadOnlyList<string> Diagonostics { get; }
        public object Value { get; }
    }
}


