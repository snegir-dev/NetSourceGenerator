using Microsoft.CodeAnalysis;
using NetSourceGenerator.Domain.Attributes;
using NetSourceGenerator.Domain.Enum;
using NetSourceGenerator.Templates;
using NetSourceGenerator.Templates.Constructors;
using NetSourceGenerator.Extensions;

namespace NetSourceGenerator.Generators.ConstructorGenerators;

[Generator]
internal class AllArgsConstructorBaseGenerator : RequiredArgsConstructorBase, IIncrementalGenerator
{
    protected override Type Type { get; } = typeof(AllArgsConstructorAttribute);
    protected override ITemplate Template { get; } = new AllArgsConstructorTemplate();

    protected override string? GeneratePartialMember(ITypeSymbol symbol)
    {
        var @namespace = symbol.ContainingNamespace.ToString();
        var dataType = GetDataType(symbol);
        var typeName = symbol.Name;
        
        var memberType = symbol.GetAttributeArgument<MemberType>() ?? MemberType.All;
        var accessType = symbol.GetAttributeArgument<AccessType>() ?? AccessType.All;

        var memberPropertiesWithType = MemberHandler
            .GetMemberNameWithType(symbol, memberType, accessType);

        if (memberPropertiesWithType.Count <= 0) return null;

        var constructor = CreateArgsConstructor(memberPropertiesWithType);

        var classWithAllArgsConstructor = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*type-object*", dataType)
            .Replace("*type-name*", typeName)
            .Replace("*params*", constructor.parameters)
            .Replace("*appropriation-params*", constructor.bodyConstructor);

        return classWithAllArgsConstructor;
    }

    protected override (string parameters, string bodyConstructor) CreateArgsConstructor(Dictionary<string,
        ITypeSymbol> memberPropertiesWithType)
    {
        var parameters = "";
        var bodyConstructor = "";

        const string paramsTemplate = @"*type* *arg-name*";
        const string bodyConstructorTemplate = @"this.*member* = *ctor-param*;";

        foreach (var member in memberPropertiesWithType)
        {
            var memberType = member.Value.Name;
            var ctorParam = member.Key.ToLower().Replace("_", string.Empty);

            parameters += paramsTemplate
                .Replace("*type*", memberType)
                .Replace("*arg-name*", ctorParam) + ", ";

            bodyConstructor += bodyConstructorTemplate
                .Replace("*member*", member.Key)
                .Replace("*ctor-param*", ctorParam) + "\n";
        }

        // remove the last comma and space
        parameters = parameters.Substring(0, parameters.Length - 2);

        return (parameters, bodyConstructor);
    }

    protected override string GetDataType(ITypeSymbol symbol)
    {
        var dataType = symbol.TypeKind switch
        {
            TypeKind.Class => symbol.TypeKind.ToString().ToLower(),
            TypeKind.Structure => "struct",
            _ => ""
        };

        return dataType;
    }
}