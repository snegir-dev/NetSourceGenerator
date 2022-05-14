namespace SourceGenerate.Templates.Constructors;

public class NoArgsConstructorTemplate : ITemplate
{
    public string GetTemplate()
    {
        return
            @"
                namespace *namespace* 
                {
                    partial class *class-name*
                    {
                        public *class-name*()
                        {
                        }
                    }
                }                
            ";
    }
}