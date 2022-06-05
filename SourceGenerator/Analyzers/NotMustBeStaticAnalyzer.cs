using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Domain.Attributes;

namespace SourceGenerator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class NotMustBeStaticAnalyzer : BaseAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptions.TypeNotMustBeStatic);

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

        var declarationSyntax = (TypeDeclarationSyntax)context.Node;

        if (!declarationSyntax.Modifiers.Any(p => p.IsKind(SyntaxKind.StaticKeyword)))
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
            DiagnosticDescriptions.TypeNotMustBeStatic,
            location,
            nameTypeIdentifier
        );

        return diagnostic;
    }
}