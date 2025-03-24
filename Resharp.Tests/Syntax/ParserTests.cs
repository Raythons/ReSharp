using System.ComponentModel;
using rs.CodeAnalysis.Syntax;
using Xunit;
namespace Resharp.Tests;

public partial class ParserTests
{

    [Theory]
    [MemberData(nameof(GetBinaryOperatorPairsData))]
    public void Parser_BinaryExpression_HonorsPrecedences(SyntaxType op1, SyntaxType op2)
    {
        var op1Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op1);
        var op2Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op2);
        var op1Text = SyntaxFacts.GetText(op1);
        var op2Text = SyntaxFacts.GetText(op2);


        var text = $"a {op1Text} b  {op2Text} c";
        var expression = SyntaxTree.Parse(text).Root;

        if (op1Precedence >= op2Precedence)
        {
            using (var e = new AssertingEnumerator(expression))
            {

                e.AssertNode(SyntaxType.BinaryExpression);
                e.AssertNode(SyntaxType.BinaryExpression);
                e.AssertNode(SyntaxType.NameExpression);
                e.AssertToken(SyntaxType.IdentifierToken, "a");

                e.AssertToken(op1, op1Text);

                e.AssertNode(SyntaxType.NameExpression);
                e.AssertToken(SyntaxType.IdentifierToken, "b");

                e.AssertToken(op2, op2Text);

                e.AssertNode(SyntaxType.NameExpression);
                e.AssertToken(SyntaxType.IdentifierToken, "c");




            }
        }
        else
        {
            using (var e = new AssertingEnumerator(expression))
            {


                e.AssertNode(SyntaxType.BinaryExpression);
                e.AssertNode(SyntaxType.NameExpression);
                e.AssertToken(SyntaxType.IdentifierToken, "a");
                e.AssertToken(op1, op1Text);

                e.AssertNode(SyntaxType.BinaryExpression);

                e.AssertNode(SyntaxType.NameExpression);
                e.AssertToken(SyntaxType.IdentifierToken, "b");
                e.AssertToken(op2, op2Text);

                e.AssertNode(SyntaxType.NameExpression);
                e.AssertToken(SyntaxType.IdentifierToken, "c");
            }
        }
    }
    public static IEnumerable<object[]> GetBinaryOperatorPairsData()
    {
        foreach (var op1 in SyntaxFacts.GetBinaryOperatorTypes())
        {
            foreach (var op2 in SyntaxFacts.GetBinaryOperatorTypes())
            {
                yield return new object[] { op1, op2 };
                yield break;
            }
        }
    }

}
