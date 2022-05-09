using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerate.Generators;

internal abstract class Generator
{
    protected abstract Type Type { get; }
    
    protected bool IsExistAttribute(SyntaxNode node, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return false;

        if (node is not ClassDeclarationSyntax classDeclaration)
            return false;

        var isFits = classDeclaration.AttributeLists
            .SelectMany(a => a.Attributes)
            .Any(a => a.Name is IdentifierNameSyntax nameSyntax &&
                      Type.Name.StartsWith(nameSyntax.Identifier.Text));

        return isFits;
    }

    protected ITypeSymbol? GetTypeSymbolOrNull(GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return null;

        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var type = (ITypeSymbol?)context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken);

        if (type == null)
            return null;

        return type.GetAttributes()
            .Select(p => p.AttributeClass)
            .Any(p => p?.Name == Type.Name)
            ? type
            : null;
    }
}