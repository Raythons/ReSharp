
namespace rs.CodeAnalysis.Binding
{
    internal sealed class BoundAssigmentExpression : BoundExpression
    {
      

        public BoundAssigmentExpression(string? name, BoundExpression expression)
        {
            Name = name;
            Expression = expression;
        }

        public override Type Type => Expression.Type;

        public override BoundNodeType BoundNodeType => BoundNodeType.AssigmentExpression;

        public string Name { get; }
        public BoundExpression Expression { get; }
    }
}