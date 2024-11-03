using System;
using System.Collections.Generic;
using System.Linq;
using EasyExceptions.Yaml.Serialization.TypeInspectors;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Applies the Yaml* attributes to another <see cref="ITypeInspector"/>.
    /// </summary>
    public sealed class YamlAttributesTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector innerTypeDescriptor;

        public YamlAttributesTypeInspector(ITypeInspector innerTypeDescriptor)
        {
            this.innerTypeDescriptor = innerTypeDescriptor;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
        {
            return innerTypeDescriptor.GetProperties(type, container)
                .Select(p => (IPropertyDescriptor)new PropertyDescriptor(p))
                .OrderBy(p => p.Order);
        }
    }
}
