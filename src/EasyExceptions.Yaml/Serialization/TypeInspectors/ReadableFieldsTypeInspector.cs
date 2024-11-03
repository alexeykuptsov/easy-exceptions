using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization.TypeInspectors
{
    /// <summary>
    /// Returns the properties and fields of a type that are readable.
    /// </summary>
    public sealed class ReadableFieldsTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeResolver typeResolver;

        public ReadableFieldsTypeInspector(ITypeResolver typeResolver)
        {
            this.typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
        {
            return type
                .GetPublicFields()
                .Select(p => (IPropertyDescriptor)new ReflectionFieldDescriptor(p, typeResolver));
        }

        private sealed class ReflectionFieldDescriptor : IPropertyDescriptor
        {
            private readonly FieldInfo fieldInfo;
            private readonly ITypeResolver typeResolver;

            public ReflectionFieldDescriptor(FieldInfo fieldInfo, ITypeResolver typeResolver)
            {
                this.fieldInfo = fieldInfo;
                this.typeResolver = typeResolver;
                ScalarStyle = ScalarStyle.Any;
            }

            public string Name => fieldInfo.Name;
            public Type Type => fieldInfo.FieldType;
            public Type? TypeOverride { get; set; }
            public int Order { get; set; }
            public bool CanWrite => !fieldInfo.IsInitOnly;
            public ScalarStyle ScalarStyle { get; set; }

            public void Write(object target, object? value)
            {
                fieldInfo.SetValue(target, value);
            }

            public T? GetCustomAttribute<T>() where T : Attribute
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(T), true);
                return (T?)attributes.FirstOrDefault();
            }

            public IObjectDescriptor Read(object target)
            {
                var propertyValue = fieldInfo.GetValue(target);
                var actualType = TypeOverride ?? typeResolver.Resolve(Type, propertyValue);
                return new ObjectDescriptor(propertyValue, actualType, Type, ScalarStyle);
            }
        }
    }
}
