
using ReSharp.CodeAnalysis;

namespace rs.CodeAnalysis.Binding
{
    internal sealed class BoundAssigmentExpression : BoundExpression
    {
      

        public BoundAssigmentExpression(VariableSymbol variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override Type Type => Variable.Type;

        public override BoundNodeType BoundNodeType => BoundNodeType.AssigmentExpression;

        public string Name { get; }
        public VariableSymbol Variable { get; }
        public BoundExpression Expression { get; }
    }
}