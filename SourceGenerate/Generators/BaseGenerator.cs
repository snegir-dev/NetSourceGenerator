using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerate.Generators;

internal abstract class BaseGenerator
{
    protected abstract Type Type { get; }
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var types = context.SyntaxProvider
            .CreateSyntaxProvider(IsExistAttribute, GetTypeSymbolOrNull)
            .Where(type => type != null)
            .Collect();
        
        context.RegisterSourceOutput(types, GenerateCode);
    }   

    private void GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols)
    {
        if (symbols.IsDefaultOrEmpty)
            return;

        foreach (var symbol in symbols)
        {
            if (symbol == null) return;

            var partialClass = GeneratePartialClass(symbol);

            context.AddSource($"{symbol.ContainingNamespace}{symbol.Name}.g.cs", partialClass);
        }
    }
    
    protected abstract string GeneratePartialClass(ITypeSymbol type);

    private bool IsExistAttribute(SyntaxNode node, CancellationToken cancellationToken)
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

    private ITypeSymbol? GetTypeSymbolOrNull(GeneratorSyntaxContext context,
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