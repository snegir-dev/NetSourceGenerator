using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace SourceGenerate.Generators;

internal interface IGenerator
{ 
    void GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols);

    string CreatePartialClass(ITypeSymbol type);
}