using System;

namespace EasyExceptions.Yaml.Serialization.TypeResolvers
{
    /// <summary>
    /// The type returned will be the actual type of the value, if available.
    /// </summary>
    public sealed class DynamicTypeResolver : ITypeResolver
    {
        public Type Resolve(Type staticType, object? actualValue)
        {
            return actualValue != null ? actualValue.GetType() : staticType;
        }
    }
}
