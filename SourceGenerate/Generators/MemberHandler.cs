using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceGenerate.Domain.Enum;

namespace SourceGenerate.Generators;

public static class MemberHandler
{
    public static Dictionary<string, ITypeSymbol> GetMemberNameWithType(ITypeSymbol type,
        MemberType memberType, AccessType accessType)
    {
        var propertiesMember = new Dictionary<string, ITypeSymbol>();

        var members = type.GetMembers()
            .Where(s => !s.IsStatic && !s.IsImplicitlyDeclared)
            .Where(s =>
            {
                return accessType switch
                {
                    AccessType.All => s.DeclaredAccessibility is Accessibility.Private or Accessibility.Public
                        or Accessibility.Protected or Accessibility.Internal,
                    AccessType.Private => s.DeclaredAccessibility == Accessibility.Private,
                    AccessType.Protected => s.DeclaredAccessibility == Accessibility.Protected,
                    AccessType.Internal => s.DeclaredAccessibility == Accessibility.Internal,
                    AccessType.Public => s.DeclaredAccessibility == Accessibility.Public,
                    _ => false
                };
            })
            .Where(s =>
            {
                return memberType switch
                {
                    MemberType.All => s.Kind is SymbolKind.Field or SymbolKind.Property,
                    MemberType.Field => s.Kind == SymbolKind.Field,
                    MemberType.Property => s.Kind == SymbolKind.Property,
                    _ => false
                };
            })
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