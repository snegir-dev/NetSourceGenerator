using Microsoft.CodeAnalysis;

namespace SourceGenerate.Generators;

public static class MemberHandler
{
    // public List<string> GetAllProperties(ITypeSymbol type)
    // {
    //     var fullMembersLine = new List<string>();
    //
    //     var members = type.GetMembers()
    //         .Where(p => !p.IsStatic &&
    //                      p.DeclaredAccessibility == Accessibility.Public &&
    //                      p.Kind == SymbolKind.Field || 
    //                      p.Kind == SymbolKind.Property)
    //         .ToList();
    //
    //     var properties = members.OfType<IPropertySymbol>().ToList();
    //     var fields = members.OfType<IFieldSymbol>().ToList();
    //
    //     foreach (var property in properties)
    //     {
    //         var fullStringProperty = $"{property.DeclaredAccessibility.ToString().ToLower()} " +
    //                            $"{property.Type.Name} {property.Name} {{ get; set; }}";
    //         
    //         fullMembersLine.Add(fullStringProperty);
    //     }
    //
    //     foreach (var field in fields)
    //     {
    //         var fullStringField = $"{field.DeclaredAccessibility.ToString().ToLower()} " +
    //                         $"{field.Type.Name} {field.Name} {{ get; set; }}";
    //         
    //         fullMembersLine.Add(fullStringField);
    //     }
    //
    //     return fullMembersLine;
    // }

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