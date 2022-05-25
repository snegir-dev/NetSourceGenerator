namespace SourceGenerator.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
[Partial]
[NoStatic]
public class SingletonAttribute : Attribute
{
    
}