﻿namespace SourceGenerate.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[Partial]
public class NoArgsConstructorAttribute : Attribute
{
    
}