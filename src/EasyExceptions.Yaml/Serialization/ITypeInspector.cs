using System;
using System.Collections.Generic;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Provides access to the properties of a type.
    /// </summary>
    public interface ITypeInspector
    {
        /// <summary>
        /// Gets all properties of the specified type.
        /// </summary>
        /// <param name="type">The type whose properties are to be enumerated.</param>
        /// <param name="container">The actual object of type <paramref name="type"/> whose properties are to be enumerated. Can be null.</param>
        /// <returns></returns>
        IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container);
    }
}
