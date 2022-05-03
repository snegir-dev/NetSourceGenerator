using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerate.Generators;

public class GeneratorHandler
{
    private readonly Type _type;

    public GeneratorHandler(Type type)
    {
        _type = type;
    }

    public bool IsExistAttribute(SyntaxNode node, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return false;

        if (node is not ClassDeclarationSyntax classDeclaration)
            return false;

        var isFits = classDeclaration.AttributeLists
            .SelectMany(a => a.Attributes)
            .Any(a => a.Name is IdentifierNameSyntax nameSyntax &&
                      _type.Name.StartsWith(nameSyntax.Identifier.Text));

        return isFits;
    }

    public ITypeSymbol? GetTypeSymbolOrNull(GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return null;

        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var type = (ITypeSymbol?)context.SemanticModel.GetDeclaredSymbol(classDeclaration);

        if (type == null)
            return null;

        return type.GetAttributes()
            .Select(p => p.AttributeClass)
            .Any(p => p?.Name == _type.Name)
            ? type
            : null;
    }

    public static void Debug<T>(IEnumerable<T> param)
    {
        var sw = new StreamWriter(@"E:\MeProgram\C#\SourceGenerate\SourceGenerate.RealTimeTests\NewFile1.txt", true);
        sw.WriteLine(string.Join("\n", param));
        sw.Dispose();
    }

    public static void Debug<T>(T param)
    {
        var sw = new StreamWriter(@"E:\MeProgram\C#\SourceGenerate\SourceGenerate.RealTimeTests\NewFile1.txt", true);
        sw.WriteLine(string.Join("\n", param));
        sw.Dispose();
    }
}