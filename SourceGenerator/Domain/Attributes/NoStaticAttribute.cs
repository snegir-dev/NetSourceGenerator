namespace SourceGenerator.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
internal class NoStaticAttribute : Attribute
{
    
}