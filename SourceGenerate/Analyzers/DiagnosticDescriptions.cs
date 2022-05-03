using Microsoft.CodeAnalysis;

namespace SourceGenerate.Analyzers;

public static class DiagnosticDescriptions
{
    public static readonly DiagnosticDescriptor TypeMustBePartial = 
        new("T001", "Type must be partial", "The type {0} must be partial", "Usage", DiagnosticSeverity.Error, true);

    public static readonly DiagnosticDescriptor TypeNotMustBePartial =
        new("T002", "Type not must be static", "The type {0} not must be static", "Usage", DiagnosticSeverity.Error, true);
}