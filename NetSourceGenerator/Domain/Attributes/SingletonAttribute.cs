namespace NetSourceGenerator.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
[Partial]
[NoStatic]
[NotMustPublicCtor]
[NotMustInternalCtor]
public class SingletonAttribute : Attribute
{
    
}