using System;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Allows to customize how a type is serialized and deserialized.
    /// </summary>
    public interface IYamlTypeConverter
    {
        /// <summary>
        /// Gets a value indicating whether the current converter supports converting the specified type.
        /// </summary>
        bool Accepts(Type type);

        /// <summary>
        /// Writes the specified object's state to a YAML emitter.
        /// </summary>
        void WriteYaml(IEmitter emitter, object? value, Type type);
    }
}
