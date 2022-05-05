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

    void IGenerator.GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols)
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
            .Replace("*params*", paramsConstructor.parameters)
            .Replace("*appropriation-params*", paramsConstructor.bodyConstructor);

        return classWithAllArgsConstructor;
    }

    private static (string parameters, string bodyConstructor) CreateParamsConstructor(Dictionary<string,
        ITypeSymbol> memberPropertiesWithType)
    {
        var parameters = "";
        var bodyConstructor = "";

        const string paramsTemplate = @"*type* *arg-name*";
        const string bodyConstructorTemplate = @"this.*member* = *ctor-param*;";

        foreach (var (memberName, value) in memberPropertiesWithType)
        {
            var memberType = value.Name;
            var ctorParam = memberName.ToLower().Replace("_", string.Empty);

            parameters += paramsTemplate
                .Replace("*type*", memberType)
                .Replace("*arg-name*", ctorParam) + ", ";

            bodyConstructor += bodyConstructorTemplate
                .Replace("*member*", memberName)
                .Replace("*ctor-param*", ctorParam) + "\n";
        }

        // remove the last comma and space
        parameters = parameters[..^2];

        return (parameters, bodyConstructor);
    }
}