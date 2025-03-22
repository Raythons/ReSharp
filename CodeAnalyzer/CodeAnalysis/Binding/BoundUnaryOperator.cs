using rs.CodeAnalysis.Syntax;

namespace rs.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator
    {

        private BoundUnaryOperator(SyntaxType syntax, BoundUnaryOperatorType operatorType, Type operandType)
            : this(syntax, operatorType, operandType, operandType)
        {
        }

        private BoundUnaryOperator(SyntaxType syntax, BoundUnaryOperatorType operatorType,
            Type operandType, Type resultType)
        {
            SyntaxType = syntax;
            OperatorType = operatorType;
            OperandType = operandType;
            Type = resultType;
        }

        public SyntaxType SyntaxType { get; }
        public BoundUnaryOperatorType OperatorType { get; }
        public Type OperandType { get; }
        public Type Type { get; }

        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxType.BadToken, BoundUnaryOperatorType.LogicalNegation, typeof(bool)),

            new BoundUnaryOperator(SyntaxType.PlusToken, BoundUnaryOperatorType.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxType.MinusToken, BoundUnaryOperatorType.Negation, typeof(int)),
        };

        public static BoundUnaryOperator Bind (SyntaxType syntaxType, Type operandType)
        {
            foreach (var oper in _operators) { 
                if(oper.SyntaxType  == syntaxType && oper.OperandType == operandType)
                    return oper;
            }
            return null;
        }
    }
}


