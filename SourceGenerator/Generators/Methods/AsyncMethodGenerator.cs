using Microsoft.CodeAnalysis;
using SourceGenerator.Domain.Attributes;
using SourceGenerator.Templates;
using SourceGenerator.Templates.Methods;
using SourceGenerator.Templates.Methods.Async;

namespace SourceGenerator.Generators.Methods;

[Generator]
internal class AsyncMethodGenerator : BaseAsyncMethodGenerator, IIncrementalGenerator
{
    protected override Type Type { get; } = typeof(AsyncAttribute);
    protected override ITemplate Template { get; } = new TypeStructureForAsyncMethodTemplate();
    protected override ITemplate MethodTemplate { get; } = new AsyncMethodTemplate();

    protected override string? GeneratePartialMember(ITypeSymbol symbol)
    {
        var @namespace = symbol.ContainingNamespace.ToString();
        var typeObject = symbol.TypeKind.ToString().ToLower();
        var typeName = symbol.Name;

        var taggedMethods = GetTaggedMethods(symbol);

        var template = Template.GetTemplate()
            .Replace("*namespace*", @namespace)
            .Replace("*type-object*", typeObject)
            .Replace("*type-name*", typeName);

        var methodTemplate = MethodTemplate.GetTemplate();
        var methods = "";

        foreach (var method in taggedMethods.Select(s => s as IMethodSymbol))
        {
            if (method == null)
                continue;

            var methodArgs = GenerateArgsMethod(method);
            var methodArgsName = GenerateArgsMethodName(method);

            var accessModifier = method.DeclaredAccessibility.ToString().ToLower();
            var returnType = method.ReturnType.SpecialType == SpecialType.System_Void
                ? "Task"
                : $"Task<{method.ReturnType}>";
            var useStatic = method.IsStatic ? "static" : "";

            methods += methodTemplate
                .Replace("*access-modifier*", accessModifier)
                .Replace("*use-static*", useStatic)
                .Replace("*return-type*", returnType)
                .Replace("*method-name*", method.Name)
                .Replace("*args*", methodArgs)
                .Replace("*args-name*", methodArgsName);
        }

        var type = template.Replace("*methods*", methods);

        return type;
    }

    protected override string GenerateArgsMethod(IMethodSymbol method)
    {
        var typeStrings = method.Parameters
            .Select(p => p.Type.ToString())
            .ToList();
        var argsNames = method.Parameters
            .Select(p => p.Name.ToString())
            .ToList();

        var argsList = new List<string>();

        for (var i = 0; i < method.Parameters.Length; i++)
        {
            argsList.Add($"{typeStrings[i]} {argsNames[i]}");
        }

        var args = string.Join(", ", argsList);

        return args;
    }

    private string GenerateArgsMethodName(IMethodSymbol method)
    {
        var argsNames = method.Parameters
            .Select(p => p.Name.ToString())
            .ToList();

        var argsList = new List<string>();

        for (var i = 0; i < method.Parameters.Length; i++)
        {
            argsList.Add($"{argsNames[i]}");
        }

        var args = string.Join(", ", argsList);

        return args;
    }
}