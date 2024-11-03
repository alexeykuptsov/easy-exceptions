using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace EasyExceptions.Yaml.Serialization.TypeInspectors
{
    /// <summary>
    /// Wraps another <see cref="ITypeInspector"/> and applies caching.
    /// </summary>
    public sealed class CachedTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector innerTypeDescriptor;
        private readonly ConcurrentDictionary<Type, List<IPropertyDescriptor>> cache = new ConcurrentDictionary<Type, List<IPropertyDescriptor>>();

        public CachedTypeInspector(ITypeInspector innerTypeDescriptor)
        {
            this.innerTypeDescriptor = innerTypeDescriptor ?? throw new ArgumentNullException(nameof(innerTypeDescriptor));
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
        {
            return cache.GetOrAdd(type, t => innerTypeDescriptor.GetProperties(t, container).ToList());
        }
    }
}
