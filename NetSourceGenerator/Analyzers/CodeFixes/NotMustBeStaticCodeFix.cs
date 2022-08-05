using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetSourceGenerator.Analyzers.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal class NotMustBeStaticCodeFix : BaseCodeFix
{
    protected sealed override string DiagnosticId { get; } = DiagnosticDescriptions.TypeNotMustBeStatic.Id;

    public override ImmutableArray<string> FixableDiagnosticIds { get; }

    public NotMustBeStaticCodeFix()
    {
        FixableDiagnosticIds = ImmutableArray.Create(DiagnosticId);
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        if (root == null)
            return;

        var typeDeclaration = (TypeDeclarationSyntax)root.FindNode(context.Span);

        var typeWithStaticModifier = typeDeclaration.Modifiers
            .FirstOrDefault(s => s.IsKind(SyntaxKind.StaticKeyword));

        var newListModifiers = typeDeclaration.Modifiers.Remove(typeWithStaticModifier);

        var newTypeDeclaration = typeDeclaration
            .WithModifiers(newListModifiers);
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
        if (declarationSyntax is not TypeDeclarationSyntax typeDeclaration)
            return null;

        var title = $"Remove 'static' from {typeDeclaration.Identifier.Text}";
        var newSyntaxNode = Task.FromResult(context.Document.WithSyntaxRoot(syntaxNode));

        var codeAction = CodeAction.Create(
            title,
            _ => newSyntaxNode,
            DiagnosticId);

        return codeAction;
    }
}