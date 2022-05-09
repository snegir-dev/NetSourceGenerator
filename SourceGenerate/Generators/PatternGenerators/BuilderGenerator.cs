using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using SourceGenerate.Domain.Attributes;
using SourceGenerate.Templates.Patterns;

namespace SourceGenerate.Generators.PatternGenerators;

[Generator]
public class BuilderGenerator : IIncrementalGenerator, IGenerator
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
            if (type == null) return;

            var builderClass = ((IGenerator)this).CreatePartialClass(type);

            context.AddSource($"{type.ContainingNamespace}{type.Name}.g.cs", builderClass);
        }
    }

    string IGenerator.CreatePartialClass(ITypeSymbol @class)
    {
        var @namespace = @class.ContainingNamespace.ToString();
        var className = @class.Name;
        var builderClassName = $"{@class.Name}Builder";

        // var propertiesMember = MemberHandler
        //     .GetMemberStringPropertiesWithType(@class, Accessibility.Public);

        var propertiesMember = new Dictionary<string, ITypeSymbol>();

        var methods = CreateMethods(propertiesMember, @class);

        var classBuilder = BuilderTemplate.Template
            .Replace("*namespace*", @namespace)
            .Replace("*class-name*", className)
            .Replace("*builder-class-name*", builderClassName)
            .Replace("*lower-class-name*", className.ToLower())
            .Replace("*methods*", methods);

        return classBuilder;
    }

    private static string CreateMethods(Dictionary<string, ITypeSymbol> propertiesMember, ITypeSymbol @class)
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