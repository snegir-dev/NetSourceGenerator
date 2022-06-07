using Microsoft.CodeAnalysis;
using SourceGenerator.Templates;

namespace SourceGenerator.Generators.PatternGenerators;

internal abstract class AdditionalMethodPatternGenerator : BaseGenerator
{
    protected abstract ITemplate MethodTemplate { get; }
    protected abstract string GenerateMethods(Dictionary<string, ITypeSymbol> propertiesMember, ITypeSymbol dataType);
}