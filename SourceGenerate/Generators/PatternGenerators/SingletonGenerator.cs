using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using SourceGenerate.Domain.Attributes;

namespace SourceGenerate.Generators.PatternGenerators;

[Generator]
public class SingletonGenerator : IIncrementalGenerator
{
    private readonly GeneratorHandler _generatorHandler = new(typeof(SingletonAttribute));

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var types = context.SyntaxProvider
            .CreateSyntaxProvider(_generatorHandler.IsExistAttribute, _generatorHandler.GetTypeSymbolOrNull)
            .Where(type => type != null)
            .Collect();

        context.RegisterSourceOutput(types, GenerateCode);
    }

    private static void GenerateCode(SourceProductionContext context, ImmutableArray<ITypeSymbol?> symbols)
    {
        if (symbols.IsDefaultOrEmpty)
            return;

        foreach (var symbol in symbols)
        {
            var partialClass = CreateSingletonClass(symbol);

            if (partialClass != null)
            {
                context.AddSource($"{symbol?.ContainingNamespace}{symbol?.Name}.g.cs", partialClass);
            }
        }
    }

    private static string? CreateSingletonClass(ITypeSymbol? symbol)
    {
        if (symbol == null) return null;

        var className = symbol.Name;
        var @namespace = symbol.ContainingNamespace.ToString()!;

        var partialClass =
            $@"
                using System;

                namespace {@namespace};

                partial class {className} 
                {{
                    private static {className} _instance;

                    private {className}()
                    {{
                    }}

                    public static {className} GetInstance()
                    {{
                        if (_instance == null)
                            _instance = new {className}();
                        return _instance;
                    }}
                }}
            ";

        return partialClass;
    }
}