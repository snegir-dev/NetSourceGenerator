using Microsoft.CodeAnalysis;

namespace SourceGenerate.Generators.PatternGenerators;

internal abstract class AdditionalMethodPatternGenerator : BaseGenerator
{
    protected abstract string GenerateMethods(Dictionary<string, ITypeSymbol> propertiesMember, ITypeSymbol @class);
}