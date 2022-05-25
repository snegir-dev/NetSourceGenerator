namespace SourceGenerator.Templates.Constructors;

public class AllArgsConstructorTemplate : ITemplate
{
    public string GetTemplate()
    {
        return
            @"
                using System;

                namespace *namespace*
                {
                    partial *type-object* *type-object-name*
                    {
                        public *type-object-name*(*params*)
                        {
                            *appropriation-params*
                        }
                    }
                }                
            ";
    }
}