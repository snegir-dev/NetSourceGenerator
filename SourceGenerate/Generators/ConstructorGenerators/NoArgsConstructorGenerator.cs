using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Templates.Constructors;

namespace SourceGenerate.Generators.ConstructorGenerators;

[Generator]
public class NoArgsConstructorGenerator : IIncrementalGenerator, IGenerator
{
    private readonly GeneratorHandler _generatorHandler = new(typeof(NoArgsConstructorAttribute));

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var types = context.SyntaxProvider
            .CreateSyntaxProvider(_generatorHandler.IsExistAttribute, _generatorHandler.GetTypeSymbolOrNull)
            .Where(t => t != null)
            .Collect();

        context.RegisterSourceOutput(types, ((IGenerator)this).GenerateCode);
    }

    void IGenerator.GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols)
    {
        if (symbols.IsDefaultOrEmpty) return;

        foreach (var type in symbols)
        {
            if (type == null) return;

            var classWithNoArgsConstructor = ((IGenerator)this).CreatePartialClass(type);

            context.AddSource($"{type.ContainingNamespace}{type.Name}.g.cs",
                classWithNoArgsConstructor);
        }
    }

    string IGenerator.CreatePartialClass(ITypeSymbol type)
    {
        var @namespace = type.ContainingNamespace.ToString();
        var className = type.Name;

        var classWithNoArgsConstructor = NoArgsConstructorTemplate.Template
            .Replace("*namespace*", @namespace)
            .Replace("*class-name*", className);

        return classWithNoArgsConstructor;
    }
}