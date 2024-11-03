using System;
using EasyExceptions.Yaml.Core;
using EasyExceptions.Yaml.Core.Events;

namespace EasyExceptions.Yaml.Serialization.Converters
{
    /// <summary>
    /// Converter for System.Guid.
    /// </summary>
    public class GuidConverter : IYamlTypeConverter
    {
        private readonly bool jsonCompatible;

        public GuidConverter(bool jsonCompatible)
        {
            this.jsonCompatible = jsonCompatible;
        }

        public bool Accepts(Type type)
        {
            return type == typeof(Guid);
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var guid = (Guid)value!;
            emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, guid.ToString("D"), jsonCompatible ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true));
        }
    }
}
