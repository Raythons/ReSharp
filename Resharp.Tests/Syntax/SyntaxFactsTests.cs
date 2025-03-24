using rs.CodeAnalysis.Syntax;

namespace Resharp.Tests;


public class SyntaxFactsTests
{
    [Theory]
    [MemberData(nameof(GetSyntaxTypesData))]

    public void SyntaxFace_GetText_RoundTrips(SyntaxType type)
    {
        var text = SyntaxFacts.GetText(type);
        if (text == null)
            return;


        var tokens = SyntaxTree.ParseTokens(text);
        var token = Assert.Single(tokens);

        Assert.Equal(type, token.Type);
        Assert.Equal(text, token.Text);
    }


    public static IEnumerable<object[]> GetSyntaxTypesData()
    {
        var types = (SyntaxType[])Enum.GetValues(typeof(SyntaxType));
        foreach (var type in types)
            yield return new object[] { type };

    }
}

