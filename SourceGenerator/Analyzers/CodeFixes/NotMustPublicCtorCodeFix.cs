using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Analyzers.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal class NotMustPublicCtorCodeFix : BaseCodeFix
{
    protected sealed override string DiagnosticId { get; } = DiagnosticDescriptions.NotMustPublicCtor.Id;
    public override ImmutableArray<string> FixableDiagnosticIds { get; }
    
    public NotMustPublicCtorCodeFix()
    {
        FixableDiagnosticIds = ImmutableArray.Create(DiagnosticId);
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        
        if (root?.FindNode(context.Span) is not ConstructorDeclarationSyntax constructorSyntax)
            return;
        
        var newSyntaxNode = root.ReplaceNode(constructorSyntax, new List<SyntaxNode>());

        var codeAction = CodeActionCreate(constructorSyntax, context, newSyntaxNode);

        if (codeAction != null)
        {
            context.RegisterCodeFix(codeAction, context.Diagnostics);
        }
    }

    protected override CodeAction? CodeActionCreate(object declarationSyntax, 
        CodeFixContext context, SyntaxNode syntaxNode)
    {
        if (declarationSyntax is not ConstructorDeclarationSyntax constructorSyntax)
            return null;

        var ctorName = constructorSyntax.Identifier.Text;
        
        var title = $"Remove '{ctorName}' constructor";
        var newSyntaxNode = Task.FromResult(context.Document.WithSyntaxRoot(syntaxNode));

        var codeAction = CodeAction.Create(
            title,
            _ => newSyntaxNode,
            DiagnosticId);

        return codeAction;
    }
}