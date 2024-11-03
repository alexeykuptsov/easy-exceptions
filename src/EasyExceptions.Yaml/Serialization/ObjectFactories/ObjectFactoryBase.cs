using System;
using System.Collections;
using System.Collections.Generic;
using EasyExceptions.Yaml.Helpers;
using EasyExceptions.Yaml.Serialization.Utilities;

namespace EasyExceptions.Yaml.Serialization.ObjectFactories
{
    public abstract class ObjectFactoryBase : IObjectFactory
    {
        public abstract object Create(Type type);

        public virtual object? CreatePrimitive(Type type) => type.IsValueType() ? Activator.CreateInstance(type) : null;

        public virtual void ExecuteOnSerialized(object value)
        {
        }

        public virtual void ExecuteOnSerializing(object value)
        {
        }

        public virtual bool GetDictionary(IObjectDescriptor descriptor, out IDictionary? dictionary, out Type[]? genericArguments)
        {
            var genericDictionaryType = ReflectionUtility.GetImplementedGenericInterface(descriptor.Type, typeof(IDictionary<,>));
            if (genericDictionaryType != null)
            {
                genericArguments = genericDictionaryType.GetGenericArguments();
                var adaptedDictionary = Activator.CreateInstance(typeof(GenericDictionaryToNonGenericAdapter<,>).MakeGenericType(genericArguments), descriptor.Value)!;
                dictionary = adaptedDictionary as IDictionary;
                return true;
            }
            genericArguments = null;
            dictionary = null;
            return false;
        }

        public virtual Type GetValueType(Type type)
        {
            var enumerableType = ReflectionUtility.GetImplementedGenericInterface(type, typeof(IEnumerable<>));
            var itemType = enumerableType != null ? enumerableType.GetGenericArguments()[0] : typeof(object);
            return itemType;
        }
    }
}
