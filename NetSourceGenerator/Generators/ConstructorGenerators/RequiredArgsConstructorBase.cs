using Microsoft.CodeAnalysis;

namespace NetSourceGenerator.Generators.ConstructorGenerators;

internal abstract class RequiredArgsConstructorBase : BaseGenerator
{
    protected abstract (string parameters, string bodyConstructor) CreateArgsConstructor(Dictionary<string,
        ITypeSymbol> memberPropertiesWithType);

    protected abstract string GetDataType(ITypeSymbol symbol);
}