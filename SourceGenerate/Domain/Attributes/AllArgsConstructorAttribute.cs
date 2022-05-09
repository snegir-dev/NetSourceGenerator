﻿using SourceGenerate.Domain.Enum;

namespace SourceGenerate.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[Partial]
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