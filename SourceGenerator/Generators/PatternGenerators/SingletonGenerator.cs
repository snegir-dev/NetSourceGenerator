using Microsoft.CodeAnalysis;
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
        var dataType = symbol.TypeKind.ToString().ToLower();
        var typeName = symbol.Name;

        var partialType = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*type-object*", dataType)
            .Replace("*type-name*", typeName);

        return partialType;
    }
}