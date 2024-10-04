namespace rs.CodeAnalysis.Syntax
{
    public enum SyntaxType
    {
        NumberToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EndOfFileToken,
        BangToken,
        AmperSandAmperSandToken,
        PipePipeToken,
        IdentifierToken,

        //Keywords
        FalseKeyword,
        TrueKeyword,

        //Expressions
        BinaryExpression,
        //NumberExpression,
        LiteralExpression,
        ParenthesizedExpression,
        UnaryExpression,
        EqualsEqualsToken,
        BangEqualsToken,
    }

}
