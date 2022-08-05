using Microsoft.CodeAnalysis;
using NetSourceGenerator.Templates;

namespace NetSourceGenerator.Generators.PatternGenerators;

internal abstract class AdditionalMethodPatternGenerator : BaseGenerator
{
    protected abstract ITemplate MethodTemplate { get; }
    protected abstract string GenerateMethods(Dictionary<string, ITypeSymbol> propertiesMember, ITypeSymbol dataType);
}