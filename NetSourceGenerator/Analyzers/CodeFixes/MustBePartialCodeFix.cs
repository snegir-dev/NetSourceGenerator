using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetSourceGenerator.Analyzers.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal class MustBePartialCodeFix : BaseCodeFix
{
    protected sealed override string DiagnosticId { get; } = DiagnosticDescriptions.TypeMustBePartial.Id;

    public override ImmutableArray<string> FixableDiagnosticIds { get; }

    public MustBePartialCodeFix()
    {
        FixableDiagnosticIds = ImmutableArray.Create(DiagnosticId);
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document
            .GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        if (root == null)
            return;

        var typeDeclaration = (TypeDeclarationSyntax)root.FindNode(context.Span);

        var newTypeDeclaration = typeDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
        var newSyntaxNode = root.ReplaceNode(typeDeclaration, newTypeDeclaration);

        var codeAction = CodeActionCreate(typeDeclaration, context, newSyntaxNode);

        if (codeAction != null)
        {
            context.RegisterCodeFix(codeAction, context.Diagnostics);
        }
    }

    protected override CodeAction? CodeActionCreate(object declarationSyntax,
        CodeFixContext context, SyntaxNode syntaxNode)
    {
        if (declarationSyntax is not TypeDeclarationSyntax typeDeclarationSyntax)
            return null;

        var title = $"Make '{typeDeclarationSyntax.Identifier.Text}' partial";
        var newSyntaxNode = Task.FromResult(context.Document.WithSyntaxRoot(syntaxNode));

        var codeAction = CodeAction.Create(
            title,
            _ => newSyntaxNode,
            DiagnosticId);

        return codeAction;
    }
}