using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceGenerate.Analyzers;

internal abstract class BaseAnalyzer : DiagnosticAnalyzer
{
    public abstract override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(Check,
            SyntaxKind.ClassDeclaration,
            SyntaxKind.StructDeclaration);
    }

    protected abstract void Check(SyntaxNodeAnalysisContext context);
}