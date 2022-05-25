namespace SourceGenerator.Templates.Patterns;

public class BuilderTemplate : ITemplate
{
    public string GetTemplate()
    {
        return
            $@"

namespace *namespace*
{{
    partial *type-object* *type-object-name*
    {{
        public static *builder-type-object-name* Builder()
        {{
            return new *builder-type-object-name*();
        }}

        public *type-object* *builder-type-object-name*
        {{
            private *type-object-name* *lower-type-object-name*;
            public *builder-type-object-name*()
            {{
                *lower-type-object-name* = new *type-object-name*();
            }}

            *methods*

            public *type-object-name* Build()
            {{
                return *lower-type-object-name*;
            }}
        }}
    }}
}}
            ";
    }
}