namespace NetSourceGenerator.Templates.Patterns.Builder;

internal class BuilderMethodTemplate : ITemplate
{
    public string GetTemplate()
    {
        return 
            @"
            public *builder-type-name*Builder Set*method-name*(*parameter*)
            {
                *lower-type-name*.*member* = *parameter-member*;
                return this;
            }
            ";
    }
}