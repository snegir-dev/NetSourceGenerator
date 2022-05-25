namespace SourceGenerator.Templates.Patterns;

public class SingletonTemplate : ITemplate
{
    public string GetTemplate()
    {
        return
            @"

namespace *namespace*
{
    partial *data-structure* *data-structure-name*
    {
        private static *data-structure-name* _instance;

        private *data-structure-name*()
        {
        }

        public static *data-structure-name* GetInstance()
        {
            if (_instance == null)
                _instance = new *data-structure-name*();
            return _instance;
        }
    }
}

            ";
    }
}