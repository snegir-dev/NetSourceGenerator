using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetSourceGenerator.Extensions;

public static class AttributeListsExtension
{
    public static bool CheckAttributeFit(this SyntaxList<AttributeListSyntax> listSyntax, Type attributeType)
    {
        return listSyntax.SelectMany(a => a.Attributes)
            .Any(a => a.Name is IdentifierNameSyntax nameSyntax &&
                      attributeType.Name.StartsWith(nameSyntax.Identifier.Text));
    }
}