using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerate.Analyzers.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal class NotMustBeStaticCodeFix : BaseCodeFix
{
    private static readonly string DiagnosticId = DiagnosticDescriptions.TypeNotMustBePartial.Id;

    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DiagnosticId);

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        if (root == null)
            return;

        var declaration = (TypeDeclarationSyntax)root.FindNode(context.Span);
        var staticModifier = declaration.Modifiers
            .FirstOrDefault(s => s.IsKind(SyntaxKind.StaticKeyword));
        var newListModifiers = declaration.Modifiers.Remove(staticModifier);

        var newDeclaration = declaration
            .WithModifiers(newListModifiers);
        var newRoot = root.ReplaceNode(declaration, newDeclaration);

        context.RegisterCodeFix(
            CodeAction.Create(
                $"Remove 'static' from {declaration.Identifier.Text}",
                c => Task.FromResult(context.Document
                    .WithSyntaxRoot(newRoot)),
                DiagnosticId),
            context.Diagnostics);
    }
}