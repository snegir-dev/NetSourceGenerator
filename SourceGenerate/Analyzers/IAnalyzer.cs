using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceGenerate.Analyzers;

internal interface IAnalyzer
{
    void Check(SyntaxNodeAnalysisContext context);
}