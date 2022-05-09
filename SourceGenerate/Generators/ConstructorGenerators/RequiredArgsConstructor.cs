using Microsoft.CodeAnalysis;

namespace SourceGenerate.Generators.ConstructorGenerators;

internal abstract class RequiredArgsConstructor : BaseConstructorGenerator
{
    protected abstract (string parameters, string bodyConstructor) CreateArgsConstructor(Dictionary<string,
        ITypeSymbol> memberPropertiesWithType);
}