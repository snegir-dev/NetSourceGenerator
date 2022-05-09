using System.Collections.Immutable;
using System.Reflection.Emit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerate.Generators.ConstructorGenerators;

internal abstract class BaseConstructorGenerator : Generator, IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var types = context.SyntaxProvider
            .CreateSyntaxProvider(IsExistAttribute, GetTypeSymbolOrNull)
            .Where(p => p != null)
            .Collect();
        
        context.RegisterSourceOutput(types, GenerateCode);
    }

    protected abstract void GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols);
    
    protected abstract string GeneratePartialClass(ITypeSymbol type);
}