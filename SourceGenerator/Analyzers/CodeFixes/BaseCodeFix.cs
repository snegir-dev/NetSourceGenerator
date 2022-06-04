using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Analyzers.CodeFixes;

internal abstract class BaseCodeFix : CodeFixProvider
{
    public override FixAllProvider? GetFixAllProvider()
    {
        return base.GetFixAllProvider();
    }

    protected abstract string DiagnosticId { get; }

    public abstract override ImmutableArray<string> FixableDiagnosticIds { get; }

    public abstract override Task RegisterCodeFixesAsync(CodeFixContext context);
    
    protected abstract CodeAction? CodeActionCreate(object declarationSyntax, 
        CodeFixContext context, SyntaxNode syntaxNode);
}