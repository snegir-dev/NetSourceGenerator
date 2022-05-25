using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Extensions;
using SourceGenerate.Templates;

namespace SourceGenerate.Generators.Methods;

internal abstract class BaseAsyncMethodGenerator : BaseGenerator
{
    protected abstract ITemplate MethodTemplate { get; }
    
    protected abstract string GenerateArgsMethod(IMethodSymbol method);

    protected override bool IsExistAttribute(SyntaxNode node, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return false;

        return node switch
        {
            ClassDeclarationSyntax classDeclaration =>
                classDeclaration.Members
                    .Any(a => a.AttributeLists.CheckAttributeFit(Type)),
            StructDeclarationSyntax structDeclaration =>
                structDeclaration.Members
                    .Any(a => a.AttributeLists.CheckAttributeFit(Type)),
            _ => false
        };
    }

    protected override ITypeSymbol? GetTypeSymbolOrNull(GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return null;

        var type = (ITypeSymbol?)(context.Node switch
        {
            ClassDeclarationSyntax classDeclaration =>
                context.SemanticModel.GetDeclaredSymbol(classDeclaration, cancellationToken),
            StructDeclarationSyntax structDeclaration =>
                context.SemanticModel.GetDeclaredSymbol(structDeclaration, cancellationToken),
            _ => null
        });

        if (type == null)
            return null;

        return type.GetMembers()
            .SelectMany(s => s.GetAttributes())
            .Select(a => a.AttributeClass)
            .Any(n => n?.Name == Type.Name)
            ? type
            : null;
    }

    protected virtual IEnumerable<ISymbol> GetTaggedMethods(ITypeSymbol symbol)
    {
        var methods = symbol.GetMembers()
            .Where(s => s.GetAttributes()
                .Any(a => a.AttributeClass?.Name == nameof(AsyncAttribute)));

        return methods;
    }
}