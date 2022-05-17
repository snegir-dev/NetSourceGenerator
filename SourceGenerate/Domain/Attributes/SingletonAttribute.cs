namespace SourceGenerate.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
[Partial]
[NoStatic]
public class SingletonAttribute : Attribute
{
    
}