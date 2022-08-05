using Microsoft.CodeAnalysis;

namespace NetSourceGenerator.Extensions;

public static class AttributeDataListExtension
{
    public static bool AnyInnerAttribute(this IEnumerable<AttributeData> attributesData, string attribute)
    {
        return attributesData.Select(a => a.AttributeClass?.GetAttributes())
            .Select(i => i!.Value
                .Any(a => a.AttributeClass?.Name == attribute))
            .Any(b => b);
    }

    public static bool AnyAttribute(this IEnumerable<AttributeData> attributesData, string attribute)
    {
        return attributesData
            .Any(a => a.AttributeClass?.Name == attribute);
    }
}