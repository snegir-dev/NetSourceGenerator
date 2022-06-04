using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Analyzers.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal class NotMustBeRefOutInModifierCodeFix : BaseCodeFix
{
    protected sealed override string DiagnosticId { get; } = DiagnosticDescriptions.ArgumentNotMustWithRefOutInModifier.Id;

    public override ImmutableArray<string> FixableDiagnosticIds { get; }
    
    public NotMustBeRefOutInModifierCodeFix()
    {
        FixableDiagnosticIds = ImmutableArray.Create(DiagnosticId);
    }
    
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        
        if (root == null)
            return;

        var parameterSyntax = (ParameterSyntax)root.FindNode(context.Span);

        var refKinds = new List<SyntaxKind>
            { SyntaxKind.RefKeyword, SyntaxKind.OutKeyword, SyntaxKind.InKeyword };
        
        var parameterWithRefModifier = parameterSyntax.Modifiers
            .FirstOrDefault(s => refKinds
                .Select(k => k == s.Kind())
                .Any());
        
        var parameterWithoutRefModifier = 
            parameterSyntax.Modifiers.Remove(parameterWithRefModifier);
        
        var newParameter = parameterSyntax
            .WithModifiers(parameterWithoutRefModifier);
        
        var newSyntaxNode = root.ReplaceNode(parameterSyntax, newParameter);

        CodeActionCreate(parameterSyntax, context, newSyntaxNode);
        
        context.RegisterCodeFix(
            CodeAction.Create(
                $"Remove '{parameterWithRefModifier.Text}' modifier",
                c => Task.FromResult(context.Document
                    .WithSyntaxRoot(newSyntaxNode)),
                DiagnosticId),
            context.Diagnostics);
    }

    protected override CodeAction? CodeActionCreate(object declarationSyntax, 
        CodeFixContext context, SyntaxNode syntaxNode)
    {
        if (declarationSyntax is not ParameterSyntax parameterSyntax)
            return null;
        
        var title = $"Remove '{parameterSyntax.Identifier.Text}' modifier";
        var newSyntaxNode = Task.FromResult(context.Document.WithSyntaxRoot(syntaxNode));

        var codeAction = CodeAction.Create(
            title,
            _ => newSyntaxNode,
            DiagnosticId);

        return codeAction;
    }
}