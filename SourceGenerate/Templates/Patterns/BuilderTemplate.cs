namespace SourceGenerate.Templates.Patterns;

public static class BuilderTemplate
{
    public static string Template => GetTemplate();
    
    private static string GetTemplate()
    {
        return
            $@"
                namespace *namespace*
                {{
                    partial class *class-name*
                    {{
                        public static *builder-class-name* Builder()
                        {{
                            return new *builder-class-name*();
                        }}
                    }}

                    public class *builder-class-name*
                    {{
                        private *class-name* *lower-class-name*;
                        public *builder-class-name*()
                        {{
                            *lower-class-name* = new *class-name*();
                        }}

                        *methods*

                        public *class-name* Build()
                        {{
                            return *lower-class-name*;
                        }}
                    }}
                }}
            ";
    }
}