using System;
using System.Collections.Generic;

namespace EasyExceptions.Yaml.Serialization.TypeInspectors
{
    public abstract class TypeInspectorSkeleton : ITypeInspector
    {
        public abstract IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container);
    }
}
