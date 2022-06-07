namespace SourceGenerator.Templates.Methods;

public class AsyncMethodTemplate : ITemplate
{
    public string GetTemplate()
    {
        return 
            @"
        *access-modifier* *use-static* *return-type* *method-name*Async(*args*)
        {
            return Task.Run(() => *method-name*(*args-name*));
        }
            ";
    }
}