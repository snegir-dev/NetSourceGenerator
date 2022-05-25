using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CodeFixes;

namespace SourceGenerator.Analyzers.CodeFixes;

internal abstract class BaseCodeFix : CodeFixProvider
{
    public override FixAllProvider? GetFixAllProvider()
    {
        return base.GetFixAllProvider();
    }

    public abstract override ImmutableArray<string> FixableDiagnosticIds { get; }

    public abstract override Task RegisterCodeFixesAsync(CodeFixContext context);
}