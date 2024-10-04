
namespace rs.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {

        public static int GetUnaryOperatorPrecedence(this SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.MinusToken:
                case SyntaxType.PlusToken:
                case SyntaxType.BangToken:
                    return 6;

                default: return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.StarToken:
                case SyntaxType.SlashToken:
                    return 5;

                case SyntaxType.PlusToken:
                case SyntaxType.MinusToken:
                    return 4;


                case SyntaxType.EqualsEqualsToken:
                case SyntaxType.BangEqualsToken:
                    return 3;

                case SyntaxType.AmperSandAmperSandToken:
                    return 2;

                case SyntaxType.PipePipeToken:
                    return 1;

                default: return 0;
            }
        }


     
        internal static SyntaxType GetKeywordType(string text)
        {
            switch (text)
            {
                case "true":
                    return SyntaxType.TrueKeyword;
                case "false":
                    return SyntaxType.FalseKeyword;
                default:
                    return SyntaxType.IdentifierToken;
            }
        }
    }

}
