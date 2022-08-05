﻿using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NetSourceGenerator.Templates;
using NetSourceGenerator.Extensions;

namespace NetSourceGenerator.Generators;

internal abstract class BaseGenerator
{
    protected abstract Type Type { get; }
    protected abstract ITemplate Template { get; }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var types = context.SyntaxProvider
            .CreateSyntaxProvider(IsExistAttribute, GetTypeSymbolOrNull)
            .Where(type => type != null)
            .Collect();

        context.RegisterSourceOutput(types, GenerateCode);
    }

    protected abstract string? GeneratePartialMember(ITypeSymbol symbol);

    private void GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols)
    {
        if (symbols.IsDefaultOrEmpty)
            return;

        foreach (var symbol in symbols)
        {
            if (symbol == null) return;

            var partialType = GeneratePartialMember(symbol);

            if (partialType != null)
            {
                context.AddSource($"{symbol.ContainingNamespace}{symbol.Name}.g.cs", partialType);
            }
        }
    }

    protected virtual bool IsExistAttribute(SyntaxNode node, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return false;

        return node switch
        {
            ClassDeclarationSyntax classDeclaration =>
                classDeclaration.AttributeLists.CheckAttributeFit(Type),
            StructDeclarationSyntax structDeclaration =>
                structDeclaration.AttributeLists.CheckAttributeFit(Type),
            _ => false
        };
    }

    protected virtual ITypeSymbol? GetTypeSymbolOrNull(GeneratorSyntaxContext context,
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

        return type.GetAttributes()
            .Select(p => p.AttributeClass)
            .Any(p => p?.Name == Type.Name)
            ? type
            : null;
    }
}