namespace SourceGenerate.Templates.Constructors;

public class AllArgsConstructorTemplate : ITemplate
{
    public string GetTemplate()
    {
        return
            @"
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