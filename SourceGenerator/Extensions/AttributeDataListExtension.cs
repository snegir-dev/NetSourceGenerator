using Microsoft.CodeAnalysis;

namespace SourceGenerator.Extensions;

public static class AttributeDataListExtension
{
    public static bool AnyAttribute(this IEnumerable<AttributeData> attributesData, string attribute)
    {
        return attributesData.Select(a => a.AttributeClass?.GetAttributes())
            .Select(i => i!.Value
                .Any(a => a.AttributeClass?.Name == attribute))
            .Any(b => b);
    }
}