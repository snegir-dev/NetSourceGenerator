using SourceGenerator.Domain.Enum;

namespace SourceGenerator.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[Partial]
[NoStatic]
public class AllArgsConstructorAttribute : Attribute
{
    public AllArgsConstructorAttribute()
    {
        
    }
    
    public AllArgsConstructorAttribute(MemberType memberType)
    {
        
    }
    
    public AllArgsConstructorAttribute(AccessType accessType)
    {
        
    }
    
    public AllArgsConstructorAttribute(MemberType memberType, AccessType accessType)
    {
        
    }
    
    public AllArgsConstructorAttribute(AccessType accessType, MemberType memberType)
    {
        
    }
}