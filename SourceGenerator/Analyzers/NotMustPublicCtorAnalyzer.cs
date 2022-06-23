using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Domain.Attributes;

namespace SourceGenerator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class NotMustPublicCtorAnalyzer : BaseAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptions.NotMustPublicCtor);

    protected override void Check(SyntaxNodeAnalysisContext context)
    {
        var nameTypeSymbol = context.Compilation
            .GetTypeByMetadataName(typeof(NotMustPublicCtorAttribute).FullName);

        if (nameTypeSymbol == null)
            return;

        var isTagged = context.ContainingSymbol?.GetAttributes()
            .Select(a => a.AttributeClass?.GetAttributes())
            .Select(i => i!.Value
                .Any(a => a.AttributeClass?.Name == nameof(NotMustPublicCtorAttribute)))
            .Any(b => b);

        if (isTagged is false or null)
            return;
        
        var declarationSyntax = (TypeDeclarationSyntax)context.Node;

        var membersDeclarationSyntax = declarationSyntax.Members
            .Where(m => m.IsKind(SyntaxKind.ConstructorDeclaration) &&
                        m.Modifiers
                            .Any(s => s.IsKind(SyntaxKind.PublicKeyword)))
            .ToList();

        if (membersDeclarationSyntax.Count != 0)
        {
            var diagnostic = CreateDiagnostic(declarationSyntax);
            if (diagnostic == null)
                return;

            context.ReportDiagnostic(diagnostic);
        }
    }

    protected override Diagnostic? CreateDiagnostic(object declarationSyntax)
    {
        if (declarationSyntax is not TypeDeclarationSyntax typeDeclarationSyntax)
            return null;

        var location = typeDeclarationSyntax.Identifier.GetLocation();
        var nameTypeIdentifier = typeDeclarationSyntax.Identifier.Text;

        var diagnostic = Diagnostic.Create(
            DiagnosticDescriptions.NotMustPublicCtor,
            location,
            nameTypeIdentifier
        );

        return diagnostic;
    }
}