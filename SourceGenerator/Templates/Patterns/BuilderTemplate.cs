namespace SourceGenerator.Templates.Patterns;

public class BuilderTemplate : ITemplate
{
    public string GetTemplate()
    {
        return
            @"
namespace *namespace*
{
    partial *type-object* *type-name*
    {
        public static *builder-type-name*Builder Builder()
        {
            return new *builder-type-name*Builder();
        }

        public *type-object* *builder-type-name*Builder
        {
            private *type-name* *lower-type-name*;
            public *builder-type-name*Builder()
            {
                *lower-type-name* = new *type-name*();
            }

            *methods*

            public *type-name* Build()
            {
                return *lower-type-name*;
            }
        }
    }
}
            ";
    }
}