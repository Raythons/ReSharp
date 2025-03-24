
namespace rs.CodeAnalysis.Syntax
{
    public static class SyntaxFacts
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


        public static IEnumerable<SyntaxType> GetUnaryOperatorTypes()
        {
            var types = (SyntaxType[])Enum.GetValues(typeof(SyntaxType));
            foreach (var type in types)
            {
                if (GetUnaryOperatorPrecedence(type) > 0)
                    yield return type;
            }
        }
        public static IEnumerable<SyntaxType> GetBinaryOperatorTypes()
        {
            var types = (SyntaxType[])Enum.GetValues(typeof(SyntaxType));
            foreach (var type in types)
            {
                if (GetBinaryOperatorPrecedence(type) > 0)
                    yield return type;
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

        public static string GetText(SyntaxType type)
        {
            switch (type)
            {
                // Operators
                case SyntaxType.PlusToken:
                    return "+";
                case SyntaxType.MinusToken:
                    return "-";
                case SyntaxType.StarToken:
                    return "*";
                case SyntaxType.SlashToken:
                    return "/";
                case SyntaxType.BangToken:
                    return "!";
                case SyntaxType.AmperSandAmperSandToken:
                    return "&&";
                case SyntaxType.PipePipeToken:
                    return "||";
                case SyntaxType.EqualsEqualsToken:
                    return "==";
                case SyntaxType.BangEqualsToken:
                    return "!=";
                case SyntaxType.EqualsToken:
                    return "=";

                // Parentheses
                case SyntaxType.OpenParenthesisToken:
                    return "(";
                case SyntaxType.CloseParenthesisToken:
                    return ")";

                // Keywords
                case SyntaxType.TrueKeyword:
                    return "true";
                case SyntaxType.FalseKeyword:
                    return "false";


                default:
                    return null;
            }
        }
    }
}
