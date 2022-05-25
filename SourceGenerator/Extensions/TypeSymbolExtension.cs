using System.ComponentModel;
using System.Reflection;
using Microsoft.CodeAnalysis;
using SourceGenerator.Domain.Enum;
using SourceGenerator.Generators;

namespace SourceGenerator.Extensions;

public static class TypeSymbolExtension
{
    public static T? GetAttributeArgument<T>(this ITypeSymbol symbol)
        where T : struct, Enum
    {
        var value = symbol.GetAttributes()
            .SelectMany(p => p.ConstructorArguments)
            .FirstOrDefault(c => c.Type?.Name == typeof(T).Name).Value;

        if (value is int enumValue)
        {
            return (T?)Enum.Parse(typeof(T), enumValue.ToString());
        }
        
        return null;
    }
}