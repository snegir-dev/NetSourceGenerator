namespace NetSourceGenerator.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[Partial]
[NoStatic]
public class BuilderAttribute : Attribute
{
    
}