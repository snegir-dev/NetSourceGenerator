namespace SourceGenerate.Templates.Constructors;

public static class AllArgsConstructorTemplate
{
    public static string Template => GetTemplate();

    private static string GetTemplate()
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