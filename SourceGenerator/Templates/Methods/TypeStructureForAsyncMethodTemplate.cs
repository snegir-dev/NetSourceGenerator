﻿namespace SourceGenerator.Templates.Methods;

public class TypeStructureForAsyncMethodTemplate : ITemplate
{
    public string GetTemplate()
    {
        return 
            @"
                using System.Threading.Tasks;

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