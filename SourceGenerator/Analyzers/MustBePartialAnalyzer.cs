using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Domain.Attributes;

namespace SourceGenerator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class MustBePartialAnalyzer : BaseAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptions.TypeMustBePartial);

    protected override void Check(SyntaxNodeAnalysisContext context)
    {
        var namedTypeSymbol = context.Compilation
            .GetTypeByMetadataName(typeof(PartialAttribute).FullName!);
        
        if (namedTypeSymbol == null)
            return;

        var isPartial = context.ContainingSymbol?.GetAttributes()
            .Select(a => a.AttributeClass?.GetAttributes())
            .Select(m => m!.Value
                .Any(a => a.AttributeClass?.Name == nameof(PartialAttribute)))
            .Any(b => b);

        if (isPartial == false)
            return;
        
        var node = (TypeDeclarationSyntax)context.Node;

        if (node.Modifiers.Any(p => p.IsKind(SyntaxKind.PartialKeyword)))
            return;

        var diagnostic = Diagnostic.Create(
            DiagnosticDescriptions.TypeMustBePartial,
            node.Identifier.GetLocation(), node.Identifier.Text);

        context.ReportDiagnostic(diagnostic);
    }
}