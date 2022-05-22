using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Domain.Enum;
using SourceGenerate.Extensions;
using SourceGenerate.Templates;
using SourceGenerate.Templates.Constructors;

namespace SourceGenerate.Generators.ConstructorGenerators;

[Generator]
internal class AllArgsConstructorBaseGenerator : RequiredArgsConstructorBase, IIncrementalGenerator
{
    protected override Type Type { get; } = typeof(AllArgsConstructorAttribute);
    protected override ITemplate Template { get; } = new AllArgsConstructorTemplate();

    protected override string? GeneratePartialMember(ITypeSymbol symbol)
    {
        var @namespace = symbol.ContainingNamespace.ToString();
        var dataStructure = GetDataStructureType(symbol);
        var className = symbol.Name;
        
        var memberType = symbol.GetAttributeArgument<MemberType>() ?? MemberType.All;
        var accessType = symbol.GetAttributeArgument<AccessType>() ?? AccessType.All;

        var memberPropertiesWithType = MemberHandler
            .GetMemberNameWithType(symbol, memberType, accessType);

        if (memberPropertiesWithType.Count <= 0) return null;

        var constructor = CreateArgsConstructor(memberPropertiesWithType);

        var classWithAllArgsConstructor = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*type-object*", dataStructure)
            .Replace("*type-object-name*", className)
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

    protected override string GetDataStructureType(ITypeSymbol symbol)
    {
        var dataStructure = "";
        
        switch (symbol.TypeKind)
        {
            case TypeKind.Class:
                dataStructure = symbol.TypeKind.ToString().ToLower();
                break;
            case TypeKind.Structure:
                dataStructure = "struct";
                break;
        }

        return dataStructure;
    }
}