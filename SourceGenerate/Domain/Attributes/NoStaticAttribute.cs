namespace SourceGenerate.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
internal class NoStaticAttribute : Attribute
{
    
}