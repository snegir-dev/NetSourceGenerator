using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerate.Domain.Attributes;

namespace SourceGenerate.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MustBePartialAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptions.TypeMustBePartial);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(CheckPartialModifier,
            SyntaxKind.ClassDeclaration,
            SyntaxKind.StructDeclaration);
    }

    private static void CheckPartialModifier(SyntaxNodeAnalysisContext context)
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