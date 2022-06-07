namespace SourceGenerator.Templates.Patterns.Singleton;

public class SingletonTemplate : ITemplate
{
    public string GetTemplate()
    {
        return
            @"
namespace *namespace*
{
    partial *type-object* *type-name*
    {
        private static *type-name* _instance;

        private *type-name*()
        {
        }

        public static *type-name* GetInstance()
        {
            if (_instance == null)
                _instance = new *type-name*();
            return _instance;
        }
    }
}
            ";
    }
}