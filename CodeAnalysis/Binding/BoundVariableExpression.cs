namespace rs.CodeAnalysis.Binding
{
    internal class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(string name, Type type)
        {
            Name = name;
            Type = type;
        }
        public string Name { get; }
        public override Type Type { get; }
        public override BoundNodeType BoundNodeType => BoundNodeType.VariableExpression;

    }
}