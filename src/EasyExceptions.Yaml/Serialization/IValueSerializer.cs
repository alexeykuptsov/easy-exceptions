using System;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization
{
    public interface IValueSerializer
    {
        void SerializeValue(IEmitter emitter, object? value, Type? type);
    }
}
