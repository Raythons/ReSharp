using System.ComponentModel;
using rs.CodeAnalysis.Syntax;
using Xunit;
using rs.CodeAnalysis.Syntax;
namespace Resharp.Tests;

public class LexerTest
{
    [Theory]
    [MemberData(nameof(GetTokensData))]
    public void Lexer_Lexes_Tokens(SyntaxType type, string text)
    {
        var tokens = SyntaxTree.ParseTokens(text);

        var token = Assert.Single(tokens);
        Assert.Equal(type, token.Type);
        Assert.Equal(text, token.Text);
    }

    // [Theory]
    [MemberData(nameof(GetTokensPairsData))]
    public void Lexer_Lexes_PairsTokens(SyntaxType t1Type, string t1Text, SyntaxType t2Type, string t2Text)
    {

        var text = t1Text + t2Text;
        var tokens = SyntaxTree.ParseTokens(text).ToArray();

        Assert.Equal(2, tokens.Length);

        Assert.Equal(tokens[0].Type, t1Type);
        Assert.Equal(tokens[0].Text, t1Text);

        Assert.Equal(tokens[1].Type, t2Type);
        Assert.Equal(tokens[1].Text, t2Text);

    }


    // [Theory]
    [MemberData(nameof(GetTokensPairsWithSeparatorData))]
    public void Lexer_Lexes_PairsTokens_WithSeparator(SyntaxType t1Type, string t1Text,
                                                    SyntaxType separatorType, string separatorText,
                                                    SyntaxType t2Type, string t2Text)
    {
        var text = t1Text + separatorText + t2Text;
        var tokens = SyntaxTree.ParseTokens(text).ToArray();

        Assert.Equal(3, tokens.Length);
        Assert.Equal(t1Type, tokens[0].Type);
        Assert.Equal(t1Text, tokens[0].Text);

        Assert.Equal(separatorType, tokens[1].Type);
        Assert.Equal(separatorText, tokens[1].Text);

        Assert.Equal(t2Type, tokens[2].Type);
        Assert.Equal(t2Text, tokens[2].Text);
    }

    public static IEnumerable<object[]> GetTokensData()
    {
        foreach (var token in GetTokens().Concat(GetSeparators()))
        {
            yield return new object[] { token.type, token.text };
        }
    }
    public static IEnumerable<object[]> GetTokensPairsData()
    {
        foreach (var token in GetTokensPairs())
        {
            yield return new object[] { token.t1Type, token.t1Text, token.t2Type, token.t2Text };
        }
    }

    public static IEnumerable<object[]> GetTokensPairsWithSeparatorData()
    {
        foreach (var token in GetTokensPairsWithSeparators())
        {
            yield return new object[] { token.t1Type, token.t1Text,
                                        token.separatorType, token.separatorText,
                                        token.t2Type, token.t2Text };
        }
    }

    private static IEnumerable<(SyntaxType type, string text)> GetTokens()
    {
        return new[] {
                        // Identifiers
            (SyntaxType.IdentifierToken, "a"),
            (SyntaxType.IdentifierToken, "abc"),
            // (SyntaxType.IdentifierToken, "test123"),
            
            // Literals
            (SyntaxType.NumberToken, "0"),
            (SyntaxType.NumberToken, "123"),
            (SyntaxType.NumberToken, "3"),

            (SyntaxType.TrueKeyword, "true"),
            (SyntaxType.FalseKeyword, "false"),
            
            // Operators
            (SyntaxType.PlusToken, "+"),
            (SyntaxType.MinusToken, "-"),
            (SyntaxType.StarToken, "*"),
            (SyntaxType.SlashToken, "/"),
            (SyntaxType.BangToken, "!"),
            (SyntaxType.AmperSandAmperSandToken, "&&"),
            (SyntaxType.PipePipeToken, "||"),
            (SyntaxType.EqualsEqualsToken, "=="),
            (SyntaxType.BangEqualsToken, "!="),
            (SyntaxType.EqualsToken, "="),
            
            // Parentheses
            (SyntaxType.OpenParenthesisToken, "("),
            (SyntaxType.CloseParenthesisToken, ")"),
            
            // Whitespace
            // (SyntaxType.WhiteSpaceToken, " "),
            // (SyntaxType.WhiteSpaceToken, "\t"),
            // (SyntaxType.WhiteSpaceToken, "\r"),
            // (SyntaxType.WhiteSpaceToken, "\n"),
            // (SyntaxType.WhiteSpaceToken, "\r\n"),
            // (SyntaxType.WhiteSpaceToken, "   ")
        };
    }

    private static IEnumerable<(SyntaxType type, string text)> GetSeparators()
    {
        return new[] {    
        // Whitespace
            (SyntaxType.WhiteSpaceToken, " "),
            (SyntaxType.WhiteSpaceToken, "\t"),
            (SyntaxType.WhiteSpaceToken, "\r"),
            (SyntaxType.WhiteSpaceToken, "\n"),
            (SyntaxType.WhiteSpaceToken, "\r\n"),
            (SyntaxType.WhiteSpaceToken, "   ")
        };
    }

    private static IEnumerable<(SyntaxType t1Type, string t1Text, SyntaxType t2Type, string t2Text)> GetTokensPairs()
    {
        foreach (var token1 in GetTokens())
        {
            foreach (var token2 in GetTokens())
            {

                if (!RequireSeparator(token1.type, token2.type))
                    yield return (token1.type, token1.text, token2.type, token2.text);
            }
        }
    }

    private static IEnumerable<(SyntaxType t1Type, string t1Text,
                                SyntaxType separatorType, string separatorText,
                                SyntaxType t2Type, string t2Text)> GetTokensPairsWithSeparators()
    {
        foreach (var token1 in GetTokens())
        {
            foreach (var token2 in GetTokens())
            {

                if (!RequireSeparator(token1.type, token2.type))
                {
                    foreach (var s in GetSeparators())
                        yield return (token1.type, token1.text, s.type, s.text, token2.type, token2.text);

                }
            }
        }
    }

    private static bool RequireSeparator(SyntaxType t1Type, SyntaxType t2Type)
    {

        var t1IsKeyWord = t1Type.ToString().Contains("Keyword");
        var t2IsKeyWord = t2Type.ToString().Contains("Keyword");

        if (t1Type == SyntaxType.IdentifierToken && t2Type == SyntaxType.IdentifierToken)
            return true;

        if (t1IsKeyWord && t2IsKeyWord)
            return true;

        if (t1IsKeyWord && t2Type == SyntaxType.IdentifierToken)
            return true;

        if (t1Type == SyntaxType.IdentifierToken && t2IsKeyWord)
            return true;


        if (t1Type == SyntaxType.NumberToken && t2Type == SyntaxType.NumberToken)
            return true;


        if (t1Type == SyntaxType.BangToken && t2Type == SyntaxType.EqualsToken)
            return true;

        if (t1Type == SyntaxType.BangToken && t2Type == SyntaxType.EqualsEqualsToken)
            return true;

        if (t1Type == SyntaxType.EqualsToken && t2Type == SyntaxType.EqualsToken)
            return true;

        if (t1Type == SyntaxType.EqualsToken && t2Type == SyntaxType.EqualsEqualsToken)
            return true;

        // TODO  add more cases
        return false;
    }
}
