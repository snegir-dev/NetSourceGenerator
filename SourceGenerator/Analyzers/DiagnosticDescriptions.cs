using Microsoft.CodeAnalysis;

namespace SourceGenerator.Analyzers;

public static class DiagnosticDescriptions
{
    public static readonly DiagnosticDescriptor TypeMustBePartial = 
        new("T001", "Type must be partial", "The type {0} must be partial", "Usage", DiagnosticSeverity.Error, true);

    public static readonly DiagnosticDescriptor TypeNotMustBeStatic =
        new("T002", "Type not must be static", "The type {0} not must be static", "Usage", DiagnosticSeverity.Error, true);
    
    public static readonly DiagnosticDescriptor ArgumentNotMustWithRefOutInModifier =
        new("T003", "Argument not must with ref, out, in modifier", "The argument {0} not must with {1} modifier", "Usage", DiagnosticSeverity.Error, true);
}