using Microsoft.CodeAnalysis;
using SourceGenerator.Domain.Attributes;
using SourceGenerator.Templates;
using SourceGenerator.Templates.Constructors;

namespace SourceGenerator.Generators.ConstructorGenerators;

[Generator]
internal class NoArgsConstructorBaseGenerator : BaseGenerator, IIncrementalGenerator
{
    protected override Type Type { get; } = typeof(NoArgsConstructorAttribute);
    protected override ITemplate Template { get; } = new NoArgsConstructorTemplate();

    protected override string GeneratePartialMember(ITypeSymbol symbol)
    {
        var @namespace = symbol.ContainingNamespace.ToString();
        var className = symbol.Name;

        var classWithNoArgsConstructor = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*class-name*", className);

        return classWithNoArgsConstructor;
    }
}