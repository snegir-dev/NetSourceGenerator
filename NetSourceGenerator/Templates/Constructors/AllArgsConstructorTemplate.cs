namespace NetSourceGenerator.Templates.Constructors;

public class AllArgsConstructorTemplate : ITemplate
{
    public string GetTemplate()
    {
        return 
            @"
using System;

namespace *namespace*
{
    partial *type-object* *type-name*
    {
        public *type-name*(*params*)
        {
            *appropriation-params*
        }
    }
}                
            ";
    }
}