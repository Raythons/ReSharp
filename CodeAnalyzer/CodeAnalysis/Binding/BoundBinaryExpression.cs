namespace rs.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundExpression Left { get; }
        public BoundBinaryOperator Op { get; }
        public BoundExpression Right { get; }
        public override Type Type => Op.Type;
        public override BoundNodeType BoundNodeType => BoundNodeType.BinaryExpression;
        
     

        public BoundBinaryExpression(BoundExpression left ,BoundBinaryOperator op, BoundExpression right)
        {
            Left = left;
            Op = op;
            Right = right;
         }
    }
}


