using System;
using System.Collections.Generic;
using System.Linq;
using EasyExceptions.Yaml.Core;
using EasyExceptions.Yaml.Serialization.TypeInspectors;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Applies the Yaml attribute overrides to another <see cref="ITypeInspector"/>.
    /// </summary>
    public sealed class YamlAttributeOverridesInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector innerTypeDescriptor;
        private readonly YamlAttributeOverrides overrides;

        public YamlAttributeOverridesInspector(ITypeInspector innerTypeDescriptor, YamlAttributeOverrides overrides)
        {
            this.innerTypeDescriptor = innerTypeDescriptor;
            this.overrides = overrides;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
        {
            var properties = innerTypeDescriptor.GetProperties(type, container);
            properties = properties
                .Select(p => (IPropertyDescriptor)new OverridePropertyDescriptor(p, overrides, type));

            return properties;
        }

        public sealed class OverridePropertyDescriptor : IPropertyDescriptor
        {
            private readonly IPropertyDescriptor baseDescriptor;
            private readonly YamlAttributeOverrides overrides;
            private readonly Type classType;

            public OverridePropertyDescriptor(IPropertyDescriptor baseDescriptor, YamlAttributeOverrides overrides, Type classType)
            {
                this.baseDescriptor = baseDescriptor;
                this.overrides = overrides;
                this.classType = classType;
            }

            public string Name => baseDescriptor.Name;

            public bool CanWrite => baseDescriptor.CanWrite;

            public Type Type => baseDescriptor.Type;

            public Type? TypeOverride
            {
                get => baseDescriptor.TypeOverride;
                set => baseDescriptor.TypeOverride = value;
            }

            public int Order
            {
                get => baseDescriptor.Order;
                set => baseDescriptor.Order = value;
            }

            public ScalarStyle ScalarStyle
            {
                get => baseDescriptor.ScalarStyle;
                set => baseDescriptor.ScalarStyle = value;
            }

            public void Write(object target, object? value)
            {
                baseDescriptor.Write(target, value);
            }

            public T? GetCustomAttribute<T>() where T : Attribute
            {
                var attr = overrides.GetAttribute<T>(classType, Name);
                return attr ?? baseDescriptor.GetCustomAttribute<T>();
            }

            public IObjectDescriptor Read(object target)
            {
                return baseDescriptor.Read(target);
            }
        }
    }
}
