namespace SourceGenerate.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[Partial]
[NoStatic]
public class SingletonAttribute : Attribute
{
    
}