using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Domain.Attributes;

namespace SourceGenerator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class NotMustInternalCtorAnalyzer : BaseAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptions.NotMustInternalCtor);

    protected override void Check(SyntaxNodeAnalysisContext context)
    {
        var nameTypeSymbol = context.Compilation
            .GetTypeByMetadataName(typeof(NotMustPublicCtorAttribute).FullName);

        if (nameTypeSymbol == null)
            return;
        
        if (context.Node is not ConstructorDeclarationSyntax constructorSyntax)
            return;
        
        var isTagged = context.ContainingSymbol?.ContainingSymbol.GetAttributes()
            .Select(a => a.AttributeClass?.GetAttributes())
            .Select(i => i!.Value
                .Any(a => a.AttributeClass?.Name == nameof(NotMustInternalCtorAttribute)))
            .Any(b => b);

        if (isTagged is false or null)
            return;

        var isExistInternalCtor = constructorSyntax.Modifiers
            .Any(s => s.IsKind(SyntaxKind.InternalKeyword));

        if (isExistInternalCtor)
        {
            var diagnostic = CreateDiagnostic(constructorSyntax);
            if (diagnostic == null)
                return;

            context.ReportDiagnostic(diagnostic);
        }
    }

    protected override Diagnostic? CreateDiagnostic(object declarationSyntax)
    {
        if (declarationSyntax is not ConstructorDeclarationSyntax constructorSyntax)
            return null;

        var location = constructorSyntax.Identifier.GetLocation();
        var nameTypeIdentifier = constructorSyntax.Identifier.Text;

        var diagnostic = Diagnostic.Create(
            DiagnosticDescriptions.NotMustInternalCtor,
            location,
            nameTypeIdentifier
        );

        return diagnostic;
    }
}