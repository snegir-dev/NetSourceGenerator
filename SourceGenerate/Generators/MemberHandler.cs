using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceGenerate.Generators;

public static class MemberHandler
{
    public static Dictionary<string, ITypeSymbol> GetMemberStringPropertiesWithType(ITypeSymbol type,
        params Accessibility[] accessModifiers)
    {
        var propertiesMember = new Dictionary<string, ITypeSymbol>();

        var members = type.GetMembers()
            .Where(p => !p.IsStatic &&
                        !p.IsImplicitlyDeclared &&
                        p.Kind == SymbolKind.Field ||
                        p.Kind == SymbolKind.Property)
            .Where(s => accessModifiers
                .Any(k => k == s.DeclaredAccessibility))
            .ToList();

        foreach (var member in members)
        {
            switch (member)
            {
                case IPropertySymbol propertySymbol:
                    propertiesMember[propertySymbol.Name] = propertySymbol.Type;
                    break;
                case IFieldSymbol fieldSymbol:
                    propertiesMember[fieldSymbol.Name] = fieldSymbol.Type;
                    break;
            }
        }

        return propertiesMember;
    }
}