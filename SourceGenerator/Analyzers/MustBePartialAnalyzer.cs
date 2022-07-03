using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Domain.Attributes;
using SourceGenerator.Extensions;

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

        var isPartial = context.ContainingSymbol?
            .GetAttributes()
            .AnyInnerAttribute(nameof(PartialAttribute));

        if (isPartial == false)
            return;

        var declarationSyntax = (TypeDeclarationSyntax)context.Node;

        if (declarationSyntax.Modifiers.Any(p => p.IsKind(SyntaxKind.PartialKeyword)))
            return;

        var diagnostic = CreateDiagnostic(declarationSyntax);
        if (diagnostic == null)
            return;

        context.ReportDiagnostic(diagnostic);
    }

    protected override Diagnostic? CreateDiagnostic(object declarationSyntax)
    {
        if (declarationSyntax is not TypeDeclarationSyntax typeDeclarationSyntax)
            return null;

        var location = typeDeclarationSyntax.Identifier.GetLocation();
        var nameTypeIdentifier = typeDeclarationSyntax.Identifier.Text;

        var diagnostic = Diagnostic.Create(
            DiagnosticDescriptions.TypeMustBePartial,
            location, 
            nameTypeIdentifier
        );

        return diagnostic;
    }
}