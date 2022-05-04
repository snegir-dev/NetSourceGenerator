using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace SourceGenerate.Generators;

public interface IGenerator
{ 
    void GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols);
}