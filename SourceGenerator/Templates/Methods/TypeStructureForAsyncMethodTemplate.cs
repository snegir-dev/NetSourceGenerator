namespace SourceGenerator.Templates.Methods;

public class TypeStructureForAsyncMethodTemplate : ITemplate
{
    public string GetTemplate()
    {
        return 
            @"
                namespace *namespace*
                {
                    partial *type-object* *type-name*
                    {
                        *methods*
                    }
                }
            ";
    }
}