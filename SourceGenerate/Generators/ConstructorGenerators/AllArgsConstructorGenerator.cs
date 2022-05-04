using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Templates.Constructors;

namespace SourceGenerate.Generators.ConstructorGenerators;

[Generator]
public class AllArgsConstructorGenerator : IIncrementalGenerator, IGenerator
{
    private readonly GeneratorHandler _generatorHandler = new(typeof(AllArgsConstructorAttribute));

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var types = context.SyntaxProvider
            .CreateSyntaxProvider(_generatorHandler.IsExistAttribute, _generatorHandler.GetTypeSymbolOrNull)
            .Where(t => t != null)
            .Collect();

        context.RegisterSourceOutput(types, ((IGenerator)this).GenerateCode);
    }

    public void GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols)
    {
        if (symbols.IsDefaultOrEmpty) return;

        foreach (var type in symbols)
        {
            if (type == null) return;

            var classWithAllArgsConstructor = ((IGenerator)this).CreatePartialClass(type);

            context.AddSource($"{type.ContainingNamespace}{type.Name}.g.cs",
                classWithAllArgsConstructor);
        }
    }

    string IGenerator.CreatePartialClass(ITypeSymbol type)
    {
        var @namespace = type.ContainingNamespace.ToString();
        var className = type.Name;

        var memberPropertiesWithType = MemberHandler
            .GetMemberStringPropertiesWithType(type, Accessibility.Public, Accessibility.Private);

        var paramsConstructor = CreateParamsConstructor(memberPropertiesWithType);

        var classWithAllArgsConstructor = AllArgsConstructorTemplate.Template
            .Replace("*namespace*", @namespace)
            .Replace("*class-name*", className)
            .Replace("*params*", paramsConstructor.args)
            .Replace("*appropriation-params*", paramsConstructor.appropriationArgs);

        return classWithAllArgsConstructor;
    }

    private (string args, string appropriationArgs) CreateParamsConstructor(Dictionary<string,
        ITypeSymbol> memberPropertiesWithType)
    {
        var @params = "";
        var appropriationParams = "";

        foreach (var memberProperty in memberPropertiesWithType)
        {
            @params += $"{memberProperty.Value} " +
                       $"{memberProperty.Key.ToLower().Replace("_", string.Empty)}, ";
            appropriationParams += $"this.{memberProperty.Key} = " +
                                   $"{memberProperty.Key.ToLower().Replace("_", string.Empty)};\n";
        }

        @params = @params[..^2];

        return (@params, appropriationParams);
    }
}