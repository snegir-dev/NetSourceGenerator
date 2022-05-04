using Microsoft.CodeAnalysis;

namespace SourceGenerate.Generators;

public static class MemberHandler
{
    public static Dictionary<string, ITypeSymbol> GetMemberPropertiesWithType(ITypeSymbol type)
    {
        var propertiesMember = new Dictionary<string, ITypeSymbol>();

        var members = type.GetMembers()
            .Where(p => !p.IsStatic &&
                        p.DeclaredAccessibility == Accessibility.Public &&
                        p.Kind == SymbolKind.Field |
                        p.Kind == SymbolKind.Property)
            .ToList();

        var t = members.Where(p => !p.IsStatic);

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