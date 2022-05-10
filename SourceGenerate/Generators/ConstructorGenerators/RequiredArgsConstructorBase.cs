using Microsoft.CodeAnalysis;

namespace SourceGenerate.Generators.ConstructorGenerators;

internal abstract class RequiredArgsConstructorBase : BaseGenerator
{
    protected abstract (string parameters, string bodyConstructor) CreateArgsConstructor(Dictionary<string,
        ITypeSymbol> memberPropertiesWithType);
}