using System;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Allows an object to customize how it is serialized and deserialized.
    /// </summary>
    public interface IYamlConvertible
    {
        /// <summary>
        /// Writes this object's state to a YAML emitter.
        /// </summary>
        /// <param name="emitter">The emitter where the object's state should be written to.</param>
        /// <param name="nestedObjectSerializer">A function that will use the current serializer to write an object to the emitter.</param>
        void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer);
    }
    /// <summary>
    /// Represents a function that is used to serialize an object of the given type.
    /// </summary>
    /// <param name="value">The object to be serialized.</param>
    /// <param name="type">
    /// The type that should be considered when emitting the object.
    /// If null, the actual type of the <paramref name="value" /> is used.
    /// </param>
    public delegate void ObjectSerializer(object? value, Type? type = null);
}
