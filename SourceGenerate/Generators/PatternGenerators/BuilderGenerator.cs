using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Templates.Patterns;

namespace SourceGenerate.Generators.PatternGenerators;

[Generator]
public class BuilderGenerator : IGenerator, IIncrementalGenerator
{
    private readonly GeneratorHandler _generatorHandler = new(typeof(BuilderAttribute));

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var types = context.SyntaxProvider
            .CreateSyntaxProvider(_generatorHandler.IsExistAttribute, _generatorHandler.GetTypeSymbolOrNull)
            .Where(p => p != null)
            .Collect();

        context.RegisterSourceOutput(types, ((IGenerator)this).GenerateCode);
    }

    void IGenerator.GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols)
    {
        if (symbols.IsDefaultOrEmpty)
            return;

        foreach (var type in symbols)
        {
            var builderClass = CreateBuilderClass(type);

            if (builderClass != null)
            {
                context.AddSource($"{type?.ContainingNamespace}{type?.Name}.g.cs", builderClass);
            }
        }
    }

    private string? CreateBuilderClass(ITypeSymbol? type)
    {
        if (type == null)
            return null;

        var @namespace = type.ContainingNamespace.ToString();
        var @class = type.Name;
        var builderClassName = $"{type.Name}Builder";
        var methods = "";
        
        var propertiesMember = MemberHandler.GetMemberPropertiesWithType(type);

        foreach (var property in propertiesMember)
        {
            var methodName = property.Key;
            methodName = methodName.Remove(1, methodName.Length - 1).ToUpper() +
                         property.Key.Remove(0, 1);

            var method =
                $@"
                    public *builder-class-name* *method-name*(*parameter*)
                    {{
                        *lower-class-name*.*property* = *lower-property*;
                        return this;
                    }}
                ";

            method = method
                .Replace("*builder-class-name*", builderClassName)
                .Replace("*method-name*", $"Set{methodName}")
                .Replace("*parameter*", $"{property.Value} {property.Key.ToLower()}")
                .Replace("*lower-class-name*", @class.ToLower())
                .Replace("*property*", property.Key)
                .Replace("*lower-property*", property.Key.ToLower());

            methods += method;
        }

        var classBuilder = BuilderTemplate.Template
            .Replace("*namespace*", @namespace)
            .Replace("*class-name*", @class)
            .Replace("*builder-class-name*", builderClassName)
            .Replace("*lower-class-name*", @class.ToLower())
            .Replace("*methods*", methods);
        
        return classBuilder;
    }
}