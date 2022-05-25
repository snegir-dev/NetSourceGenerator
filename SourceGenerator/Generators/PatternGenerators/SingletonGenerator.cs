using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using SourceGenerator.Domain.Attributes;
using SourceGenerator.Templates;
using SourceGenerator.Templates.Patterns;

namespace SourceGenerator.Generators.PatternGenerators;

[Generator]
internal class SingletonGenerator : BaseGenerator, IIncrementalGenerator
{
    protected override Type Type { get; } = typeof(SingletonAttribute);
    protected override ITemplate Template { get; } = new SingletonTemplate();

    protected override string GeneratePartialMember(ITypeSymbol symbol)
    {
        var @namespace = symbol.ContainingNamespace.ToString()!;
        var dataStructure = symbol.TypeKind.ToString().ToLower();
        var dataStructureName = symbol.Name;

        var partialClass = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*data-structure*", dataStructure)
            .Replace("*data-structure-name*", dataStructureName);

        return partialClass;
    }
}