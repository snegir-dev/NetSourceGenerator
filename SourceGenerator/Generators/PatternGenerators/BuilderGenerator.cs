using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using SourceGenerator.Domain.Attributes;
using SourceGenerator.Domain.Enum;
using SourceGenerator.Templates;
using SourceGenerator.Templates.Patterns;

namespace SourceGenerator.Generators.PatternGenerators;

[Generator]
internal class BuilderGenerator : AdditionalMethodPatternGenerator, IIncrementalGenerator
{
    protected override Type Type { get; } = typeof(BuilderAttribute);
    protected override ITemplate Template { get; } = new BuilderTemplate();

    protected override string GeneratePartialMember(ITypeSymbol symbol)
    {
        var @namespace = symbol.ContainingNamespace.ToString();
        var dataStructure = "";
        var className = symbol.Name;
        var builderClassName = $"{symbol.Name}Builder";

        dataStructure = symbol.TypeKind switch
        {
            TypeKind.Class => symbol.TypeKind.ToString().ToLower(),
            TypeKind.Structure => "struct",
            _ => dataStructure
        };

        var propertiesMember = MemberHandler
            .GetMemberNameWithType(symbol, MemberType.All, AccessType.All);

        var methods = GenerateMethods(propertiesMember, symbol);

        var classBuilder = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*type-object*", dataStructure)
            .Replace("*type-object-name*", className)
            .Replace("*builder-type-object-name*", builderClassName)
            .Replace("*lower-type-object-name*", className.ToLower())
            .Replace("*methods*", methods);

        return classBuilder;
    }

    protected override string GenerateMethods(Dictionary<string, ITypeSymbol> propertiesMember, 
        ITypeSymbol @class)
    {
        var methods = "";

        const string methodTemplate = @"
                    public *builder-type-object-name* *method-name*(*parameter*)
                    {
                        *lower-type-object-name*.*member* = *parameter-member*;
                        return this;
                    }
                ";

        var builderClassName = $"{@class.Name}Builder";

        foreach (var member in propertiesMember)
        {
            var memberName = member.Key.Replace("_", "");
            var methodName = new StringBuilder(memberName)
            {
                [0] = Convert.ToChar(memberName[0].ToString().ToUpper())
            };

            var parameter = $"{member.Value} {member.Key.ToLower()}";
            var lowerClassMame = @class.Name.ToLower();
            var parameterMember = member.Key.ToLower();

            methods += methodTemplate
                .Replace("*builder-type-object-name*", builderClassName)
                .Replace("*method-name*", $"Set{methodName}")
                .Replace("*parameter*", parameter)
                .Replace("*lower-type-object-name*", lowerClassMame)
                .Replace("*member*", member.Key)
                .Replace("*parameter-member*", parameterMember);
        }

        return methods;
    }
}
