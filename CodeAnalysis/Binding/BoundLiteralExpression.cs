namespace rs.CodeAnalysis.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpression
    {
        BoundUnaryOperatorType OperatorType { get; }
        BoundExpression Operand { get; }

        public override Type Type => Value.GetType();

        public override BoundNodeType BoundNodeType => BoundNodeType.LiteralExpression;

        public object Value { get; }

        public BoundLiteralExpression(object value)
        {
            Value = value;
        }
    }
}


