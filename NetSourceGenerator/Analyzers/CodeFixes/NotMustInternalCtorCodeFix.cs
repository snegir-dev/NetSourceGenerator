using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetSourceGenerator.Analyzers.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal class NotMustInternalCtorCodeFix : NotMustPublicCtorCodeFix
{
    protected sealed override string DiagnosticId { get; } = DiagnosticDescriptions.NotMustInternalCtor.Id;
    public override ImmutableArray<string> FixableDiagnosticIds { get; }
    
    public NotMustInternalCtorCodeFix()
    {
        FixableDiagnosticIds = ImmutableArray.Create(DiagnosticId);
    }
}