using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Templates.Constructors;

namespace SourceGenerate.Generators.ConstructorGenerators;

[Generator]
internal class NoArgsConstructorBaseGenerator : BaseGenerator
{
    protected override Type Type { get; } = typeof(NoArgsConstructorAttribute);

    // protected override void GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols)
    // {
    //     if (symbols.IsDefaultOrEmpty) return;
    //
    //     foreach (var type in symbols)
    //     {
    //         if (type == null) return;
    //
    //         var partialClass = GeneratePartialClass(type);
    //
    //         context.AddSource($"{type.ContainingNamespace}{type.Name}.g.cs",
    //             partialClass);
    //     }
    // }

    protected override string GeneratePartialClass(ITypeSymbol type)
    {
        var @namespace = type.ContainingNamespace.ToString();
        var className = type.Name;

        var classWithNoArgsConstructor = NoArgsConstructorTemplate.Template
            .Replace("*namespace*", @namespace)
            .Replace("*class-name*", className);

        return classWithNoArgsConstructor;
    }
}