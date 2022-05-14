namespace SourceGenerate.Templates.Patterns;

public class SingletonTemplate : ITemplate
{
    public string GetTemplate()
    {
        return
            @"
                using System;

                namespace *namespace*
                {

                    partial class *class-name*
                    {
                        private static *class-name* _instance;

                        private *class-name*()
                        {
                        }

                        public static *class-name* GetInstance()
                        {
                            if (_instance == null)
                                _instance = new *class-name*();
                            return _instance;
                        }
                    }
                }
            ";
    }
}