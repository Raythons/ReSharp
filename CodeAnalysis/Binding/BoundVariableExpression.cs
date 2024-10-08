using ReSharp.CodeAnalysis;

namespace rs.CodeAnalysis.Binding
{
    internal class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(VariableSymbol variable)
        {

            Variable = variable;
        }
        public override Type Type => Variable.Type;
        public override BoundNodeType BoundNodeType => BoundNodeType.VariableExpression;

        public VariableSymbol Variable { get; }
    }
}