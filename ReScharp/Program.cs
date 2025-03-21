using rs.CodeAnalysis.Binding;
using rs.CodeAnalysis.Syntax;
using ReSharp.CodeAnalysis;
using System.Drawing;
using System.IO.Pipes;

namespace ReSharp
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            bool showTree = false;
            var variables = new Dictionary<VariableSymbol, object>();
            while (true)
            {
                Console.Write("> ");
                var Line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(Line))
                    return;

                if (Line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing The Parse Tree" : "Not Showing The Parse Tree");
                    continue;
                }

                var syntaxTree = SyntaxTree.Parse(Line);
                var compilation = new Compilation(syntaxTree);

                var result = compilation.Evaluate(variables);
                var diagnostics = result.Diagonostics;

                if (showTree)
                {
                    var Color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (!diagnostics.Any())
                {
                    Console.WriteLine(result.Value);
                }
                else
                {

                    foreach (var diagnostic in result.Diagonostics)
                    {
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefix = Line.Substring(0, diagnostic.Span.Start);
                        var error = Line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        var suffix = Line.Substring(diagnostic.Span.End);

                        Console.Write("    ");
                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();

                        Console.Write(suffix);

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }
                Console.ResetColor();
            }

            static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
            {

                var marker = isLast ? "└──" : "├──";
                Console.Write(indent);
                Console.Write(marker);
                Console.Write(node.Type);

                if (node is SyntaxToken t)
                {
                    Console.Write(" ");
                    Console.Write(t.Value ?? t.Text);
                }

                Console.WriteLine();

                indent += isLast ? "   " : "|   ";

                // Getting Last Node To Know if Its The last one :3
                var lastChild = node.GetChildren().LastOrDefault();

                foreach (var child in node.GetChildren())
                {
                    PrettyPrint(child, indent, child == lastChild);
                }
            }
        }

    }
}