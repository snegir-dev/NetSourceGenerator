namespace SourceGenerate.Templates.Constructors;

public class AllArgsConstructorTemplate : ITemplate
{
    public string GetTemplate()
    {
        return
            @"
                namespace *namespace*
                {
                    partial class *class-name*
                    {
                        public *class-name*(*params*)
                        {
                            *appropriation-params*
                        }
                    }
                }                
            ";
    }
}