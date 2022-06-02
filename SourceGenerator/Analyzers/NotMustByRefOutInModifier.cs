﻿using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Domain.Attributes;

namespace SourceGenerator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class NotMustByRefOutInModifier : BaseAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptions.ArgumentNotMustWithRefOutInModifier);

    protected override void Check(SyntaxNodeAnalysisContext context)
    {
        var namedTypeSymbol = context.Compilation
            .GetTypeByMetadataName(typeof(AsyncAttribute).FullName!);
        
        if (namedTypeSymbol == null)
            return;

        var symbol = context.ContainingSymbol;
        
        if (context.Node is not ParameterSyntax parameterSyntax || symbol == null)
            return;

        var refKinds = new List<SyntaxKind> { SyntaxKind.RefKeyword, SyntaxKind.OutKeyword, SyntaxKind.InKeyword };

        var isTagged = symbol.GetAttributes()
            .Any(a => a.AttributeClass?.Name == nameof(AsyncAttribute));

        var isContainRef = parameterSyntax.Modifiers
            .Select(s => refKinds
                .Any(k => k == s.Kind()))
            .Any(b => b);

        if (isTagged && isContainRef)
        {
            var refKind = parameterSyntax.Modifiers
                .FirstOrDefault().Kind().ToString().Replace("Keyword", "").ToLower();
            
            var diagnostic = Diagnostic.Create(
                DiagnosticDescriptions.ArgumentNotMustWithRefOutInModifier,
                parameterSyntax.GetLocation(), 
                $"'{parameterSyntax.Identifier}'", 
                $"'{refKind}'");

            context.ReportDiagnostic(diagnostic);
        }
    }
}