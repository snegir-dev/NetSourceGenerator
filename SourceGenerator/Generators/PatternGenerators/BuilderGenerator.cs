using System.Text;
using Microsoft.CodeAnalysis;
using SourceGenerator.Domain.Attributes;
using SourceGenerator.Domain.Enum;
using SourceGenerator.Templates;
using SourceGenerator.Templates.Patterns;
using SourceGenerator.Templates.Patterns.Builder;

namespace SourceGenerator.Generators.PatternGenerators;

[Generator]
internal class BuilderGenerator : AdditionalMethodPatternGenerator, IIncrementalGenerator
{
    protected override Type Type { get; } = typeof(BuilderAttribute);
    protected override ITemplate Template { get; } = new BuilderTemplate();
    protected override ITemplate MethodTemplate { get; } = new BuilderMethodTemplate();

    protected override string GeneratePartialMember(ITypeSymbol symbol)
    {
        var @namespace = symbol.ContainingNamespace.ToString();
        var typeName = symbol.Name;
        var builderTypeName = symbol.Name;

        var dataType = symbol.TypeKind switch
        {
            TypeKind.Class => symbol.TypeKind.ToString().ToLower(),
            TypeKind.Structure => "struct",
            _ => null
        };

        var propertiesMember = MemberHandler
            .GetMemberNameWithType(symbol, MemberType.All, AccessType.All);

        var methods = GenerateMethods(propertiesMember, symbol);

        var objectType = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*type-object*", dataType)
            .Replace("*type-name*", typeName)
            .Replace("*builder-type-name*", builderTypeName)
            .Replace("*lower-type-name*", typeName.ToLower())
            .Replace("*methods*", methods);

        return objectType;
    }

    protected override string GenerateMethods(Dictionary<string, ITypeSymbol> propertiesMember, 
        ITypeSymbol dataType)
    {
        var methods = "";

        var dataTypeName = dataType.Name;

        foreach (var member in propertiesMember)
        {
            var memberName = member.Key.Replace("_", "");
            var methodName = new StringBuilder(memberName)
            {
                [0] = Convert.ToChar(memberName[0].ToString().ToUpper())
            };

            var parameter = $"{member.Value} {member.Key.ToLower()}";
            var parameterMember = member.Key.ToLower();

            methods += MethodTemplate.GetTemplate()
                .Replace("*builder-type-name*", dataTypeName)
                .Replace("*method-name*", methodName.ToString())
                .Replace("*parameter*", parameter)
                .Replace("*lower-type-name*", dataTypeName.ToLower())
                .Replace("*member*", member.Key)
                .Replace("*parameter-member*", parameterMember);
        }

        return methods;
    }
}
