using System;
using System.Collections;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Creates instances of types.
    /// </summary>
    /// <remarks>
    /// This interface allows to provide a custom logic for creating instances during deserialization.
    /// </remarks>
    public interface IObjectFactory
    {
        /// <summary>
        /// Creates a default value for the .net primitive types (string, int, bool, etc)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object? CreatePrimitive(Type type);

        /// <summary>
        /// If the type is convertable to a non generic dictionary, then it will do so and set dictionary and genericArguments to the correct values and return true.
        /// If not, values will be null and the result will be false..
        /// </summary>
        /// <param name="descriptor">Object descriptor to try and convert</param>
        /// <param name="dictionary">The converted dictionary</param>
        /// <param name="genericArguments">Generic type arguments that specify the key and value type</param>
        /// <returns>True if converted, false if not</returns>
        bool GetDictionary(IObjectDescriptor descriptor, out IDictionary? dictionary, out Type[]? genericArguments);

        /// <summary>
        /// Gets the type of the value part of a dictionary or list.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type GetValueType(Type type);

        /// <summary>
        /// Executes the methods on the object that has the <seealso cref="Callbacks.OnSerializingAttribute"/> attribute
        /// </summary>
        /// <param name="value"></param>
        void ExecuteOnSerializing(object value);

        /// <summary>
        /// Executes the methods on the object that has the <seealso cref="Callbacks.OnSerializedAttribute"/> attribute
        /// </summary>
        /// <param name="value"></param>
        void ExecuteOnSerialized(object value);
    }
}
