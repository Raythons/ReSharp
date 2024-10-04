using rs.CodeAnalysis.Syntax;

namespace rs.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {

        private BoundBinaryOperator(SyntaxType syntax, BoundBinaryOperatorType operatorType, Type operandType ,Type resultType)
          : this(syntax, operatorType, operandType, operandType, resultType)
        {
        }

        private BoundBinaryOperator(SyntaxType syntax, BoundBinaryOperatorType operatorType, Type type)
            : this(syntax, operatorType, type, type, type)
        {
        }

        private BoundBinaryOperator(SyntaxType syntax, BoundBinaryOperatorType operatorType, Type leftType,
            Type rightType, Type resultType)
        {
            SyntaxType = syntax;
            OperatorType = operatorType;
            LeftType = leftType;
            RightType = rightType;
            Type = resultType;
           
        }

        public SyntaxType SyntaxType { get; }
        public BoundBinaryOperatorType OperatorType { get; }
        public  Type LeftType { get; }
        public  Type RightType {  get; }
        public Type Type { get; }


        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxType.PlusToken, BoundBinaryOperatorType.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxType.MinusToken, BoundBinaryOperatorType.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxType.SlashToken, BoundBinaryOperatorType.Division, typeof(int)),
            new BoundBinaryOperator(SyntaxType.StarToken, BoundBinaryOperatorType.Multiplication, typeof(int)),

            //new BoundBinaryOperator(SyntaxType.PlusToken, BoundBinaryOperatorType.Addition, typeof(bool)),
            //new BoundBinaryOperator(SyntaxType.MinusToken, BoundBinaryOperatorType.Subtraction, typeof(bool)),
            //new BoundBinaryOperator(SyntaxType.SlashToken, BoundBinaryOperatorType.Division, typeof(bool)),
            //new BoundBinaryOperator(SyntaxType.StarToken, BoundBinaryOperatorType.Multiplication, typeof(bool)),

            new BoundBinaryOperator(SyntaxType.EqualsEqualsToken, BoundBinaryOperatorType.Equals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxType.BangEqualsToken, BoundBinaryOperatorType.NotEquals, typeof(int), typeof(bool)),

            new BoundBinaryOperator(SyntaxType.EqualsEqualsToken, BoundBinaryOperatorType.Equals, typeof(bool)),
            new BoundBinaryOperator(SyntaxType.BangEqualsToken, BoundBinaryOperatorType.NotEquals, typeof(bool)),



            new BoundBinaryOperator(SyntaxType.AmperSandAmperSandToken, BoundBinaryOperatorType.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxType.PipePipeToken, BoundBinaryOperatorType.LogicalOr, typeof(bool)),

        };


        public static BoundBinaryOperator Bind(SyntaxType syntaxType, Type leftType, Type rgihtType)
        {
            foreach (var oper in _operators)
            {
                if (oper.SyntaxType == syntaxType && oper.LeftType == leftType && oper.RightType == rgihtType)
                    return oper;
            }
            return null;
        }
    }
}


