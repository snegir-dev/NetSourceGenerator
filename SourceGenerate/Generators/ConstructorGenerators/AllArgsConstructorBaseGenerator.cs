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
        var className = symbol.Name;

        var memberType = symbol.GetAttributeArgument<MemberType>() ?? MemberType.All;
        var accessType = symbol.GetAttributeArgument<AccessType>() ?? AccessType.All;

        var memberPropertiesWithType = MemberHandler
            .GetMemberNameWithType(symbol, memberType, accessType);

        if (memberPropertiesWithType.Count <= 0) return null;
        
        FileDebug.Debug(memberType + " - " + accessType);

        var constructor = CreateArgsConstructor(memberPropertiesWithType);

        var classWithAllArgsConstructor = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*class-name*", className)
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