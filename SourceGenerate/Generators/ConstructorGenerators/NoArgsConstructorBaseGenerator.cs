using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Templates;
using SourceGenerate.Templates.Constructors;

namespace SourceGenerate.Generators.ConstructorGenerators;

[Generator]
internal class NoArgsConstructorBaseGenerator : BaseGenerator, IIncrementalGenerator
{
    protected override Type Type { get; } = typeof(NoArgsConstructorAttribute);
    protected override ITemplate Template { get; } = new NoArgsConstructorTemplate();

    protected override string GeneratePartialClass(ITypeSymbol type)
    {
        var @namespace = type.ContainingNamespace.ToString();
        var className = type.Name;

        var classWithNoArgsConstructor = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*class-name*", className);

        return classWithNoArgsConstructor;
    }
}