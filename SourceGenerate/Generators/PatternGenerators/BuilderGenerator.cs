using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Domain.Enum;
using SourceGenerate.Templates;
using SourceGenerate.Templates.Patterns;

namespace SourceGenerate.Generators.PatternGenerators;

[Generator]
internal class BuilderGenerator : AdditionalMethodPatternGenerator, IIncrementalGenerator
{
    protected override Type Type { get; } = typeof(BuilderAttribute);
    protected override ITemplate Template { get; } = new BuilderTemplate();

    protected override string GeneratePartialMember(ITypeSymbol @class)
    {
        var @namespace = @class.ContainingNamespace.ToString();
        var className = @class.Name;
        var builderClassName = $"{@class.Name}Builder";

        var propertiesMember = MemberHandler
            .GetMemberNameWithType(@class, MemberType.All, AccessType.All);

        var methods = GenerateMethods(propertiesMember, @class);

        var classBuilder = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*class-name*", className)
            .Replace("*builder-class-name*", builderClassName)
            .Replace("*lower-class-name*", className.ToLower())
            .Replace("*methods*", methods);

        return classBuilder;
    }

    protected override string GenerateMethods(Dictionary<string, ITypeSymbol> propertiesMember, ITypeSymbol @class)
    {
        var methods = "";

        const string methodTemplate = @"
                    public *builder-class-name* *method-name*(*parameter*)
                    {
                        *lower-class-name*.*member* = *parameter-member*;
                        return this;
                    }
                ";

        var builderClassName = $"{@class.Name}Builder";

        foreach (var member in propertiesMember)
        {
            var methodName = new StringBuilder(member.Key)
            {
                [0] = Convert.ToChar(member.Key[0].ToString().ToUpper())
            };

            var parameter = $"{member.Value} {member.Key.ToLower()}";
            var lowerClassMame = @class.Name.ToLower();
            var parameterMember = member.Key.ToLower();

            methods += methodTemplate
                .Replace("*builder-class-name*", builderClassName)
                .Replace("*method-name*", $"Set{methodName}")
                .Replace("*parameter*", parameter)
                .Replace("*lower-class-name*", lowerClassMame)
                .Replace("*member*", member.Key)
                .Replace("*parameter-member*", parameterMember);
        }

        return methods;
    }
}