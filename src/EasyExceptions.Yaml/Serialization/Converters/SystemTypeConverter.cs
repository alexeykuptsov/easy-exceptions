using System;
using EasyExceptions.Yaml.Core;
using EasyExceptions.Yaml.Core.Events;

namespace EasyExceptions.Yaml.Serialization.Converters
{
    /// <summary>
    /// Converter for System.Type.
    /// </summary>
    /// <remarks>
    /// Converts <see cref="System.Type" /> to a scalar containing the assembly qualified name of the type.
    /// </remarks>
    public class SystemTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(Type).IsAssignableFrom(type);
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var systemType = (Type)value!;
            emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, systemType.AssemblyQualifiedName!, ScalarStyle.Any, true));
        }
    }
}
