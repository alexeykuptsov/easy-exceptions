using System;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Resolves the type of values.
    /// </summary>
    public interface ITypeResolver
    {
        Type Resolve(Type staticType, object? actualValue);
    }
}
