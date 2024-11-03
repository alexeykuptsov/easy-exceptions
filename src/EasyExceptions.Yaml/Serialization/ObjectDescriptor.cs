using System;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization
{
    public sealed class ObjectDescriptor : IObjectDescriptor
    {
        public object? Value { get; }
        public Type Type { get; }
        public Type StaticType { get; }
        public ScalarStyle ScalarStyle { get; }

        public ObjectDescriptor(object? value, Type type, Type staticType, ScalarStyle scalarStyle = ScalarStyle.Any)
        {
            Value = value;
            Type = type ?? throw new ArgumentNullException(nameof(type));
            StaticType = staticType ?? throw new ArgumentNullException(nameof(staticType));

            ScalarStyle = scalarStyle;
        }
    }
}
