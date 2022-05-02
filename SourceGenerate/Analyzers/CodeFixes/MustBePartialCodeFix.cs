using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerate.Analyzers.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp)]
public class MustBePartialCodeFix : CodeFixProvider
{
    private static readonly string DiagnosticId = DiagnosticDescriptions.TypeMustBePartial.Id;

    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DiagnosticId);

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document
            .GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        if (root == null)
            return;

        var declaration = (TypeDeclarationSyntax)root.FindNode(context.Span);

        var newDeclaration = declaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
        var newRoot = root.ReplaceNode(declaration, newDeclaration);

        context.RegisterCodeFix(
            CodeAction.Create(
                $"Make '{declaration.Identifier.Text}' partial",
                c => Task.FromResult(context.Document
                    .WithSyntaxRoot(newRoot)),
                DiagnosticId
            ),
            context.Diagnostics);
    }
}