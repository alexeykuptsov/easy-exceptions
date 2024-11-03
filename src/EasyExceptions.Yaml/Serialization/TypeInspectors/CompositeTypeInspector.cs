using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyExceptions.Yaml.Serialization.TypeInspectors
{
    /// <summary>
    /// Aggregates the results from multiple <see cref="ITypeInspector" /> into a single one.
    /// </summary>
    public sealed class CompositeTypeInspector : TypeInspectorSkeleton
    {
        private readonly IEnumerable<ITypeInspector> typeInspectors;

        public CompositeTypeInspector(params ITypeInspector[] typeInspectors)
            : this((IEnumerable<ITypeInspector>)typeInspectors)
        {
        }

        public CompositeTypeInspector(IEnumerable<ITypeInspector> typeInspectors)
        {
            this.typeInspectors = typeInspectors?.ToList() ?? throw new ArgumentNullException(nameof(typeInspectors));
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
        {
            return typeInspectors
                .SelectMany(i => i.GetProperties(type, container));
        }
    }
}
