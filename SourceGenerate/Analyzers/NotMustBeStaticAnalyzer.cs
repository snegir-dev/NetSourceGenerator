using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Generators;

namespace SourceGenerate.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NotMustBeStaticAnalyzer : DiagnosticAnalyzer, IAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptions.TypeNotMustBePartial);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | 
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);
        
        context.RegisterSyntaxNodeAction(((IAnalyzer)this).Check, 
            SyntaxKind.ClassDeclaration, 
            SyntaxKind.StructDeclaration);
    }

    void IAnalyzer.Check(SyntaxNodeAnalysisContext context)
    {
        var nameTypeSymbol = context.Compilation
            .GetTypeByMetadataName(typeof(NoStaticAttribute).FullName!);

        if (nameTypeSymbol == null)
            return;
        
        var isStatic = context.ContainingSymbol?.GetAttributes()
            .Select(a => a.AttributeClass?.GetAttributes())
            .Select(i => i!.Value
                .Any(a => a.AttributeClass?.Name == nameof(NoStaticAttribute)))
            .Any(b => b);
        

        if (isStatic == false)
            return;
        
        var node = (TypeDeclarationSyntax)context.Node;

        if (!node.Modifiers.Any(p => p.IsKind(SyntaxKind.StaticKeyword)))
            return;

        var diagnostic = Diagnostic.Create(
            DiagnosticDescriptions.TypeNotMustBePartial,
            node.Identifier.GetLocation(), node.Identifier.Text);
        
        context.ReportDiagnostic(diagnostic);
    }
}