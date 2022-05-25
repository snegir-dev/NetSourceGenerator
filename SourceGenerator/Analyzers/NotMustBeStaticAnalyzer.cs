using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Generators;
using SourceGenerator.Domain.Attributes;

namespace SourceGenerator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class NotMustBeStaticAnalyzer : BaseAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptions.TypeNotMustBePartial);

    protected override void Check(SyntaxNodeAnalysisContext context)
    {
        var nameTypeSymbol = context.Compilation
            .GetTypeByMetadataName(typeof(NoStaticAttribute).FullName!);

        if (nameTypeSymbol == null)
            return;
        
        var isStatic = context.ContainingSymbol?.GetAttributes()
            .Select(a => a.AttributeClass?.GetAttributes())
            .Select(i => i!.Value
                .Any(a => a.AttributeClass?.Name == nameof(NoStaticAttribute)))
            .Any(b => b);
        

        if (isStatic == false)
            return;
        
        var node = (TypeDeclarationSyntax)context.Node;

        if (!node.Modifiers.Any(p => p.IsKind(SyntaxKind.StaticKeyword)))
            return;

        var diagnostic = Diagnostic.Create(
            DiagnosticDescriptions.TypeNotMustBePartial,
            node.Identifier.GetLocation(), node.Identifier.Text);
        
        context.ReportDiagnostic(diagnostic);
    }
}