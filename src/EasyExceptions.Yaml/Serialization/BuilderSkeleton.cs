using System;
using System.Collections.Generic;
using EasyExceptions.Yaml.Serialization.Converters;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Common implementation of <see cref="SerializerBuilder" /> and <see cref="DeserializerBuilder" />.
    /// </summary>
    public abstract class BuilderSkeleton<TBuilder>
        where TBuilder : BuilderSkeleton<TBuilder>
    {
        internal ITypeResolver typeResolver;
        internal readonly YamlAttributeOverrides overrides;
        internal readonly LazyComponentRegistrationList<Nothing, IYamlTypeConverter> typeConverterFactories;
        internal readonly LazyComponentRegistrationList<ITypeInspector, ITypeInspector> typeInspectorFactories;
        internal bool includeNonPublicProperties = false;
        internal YamlFormatter yamlFormatter = YamlFormatter.Default;

        internal BuilderSkeleton(ITypeResolver typeResolver)
        {
            overrides = new YamlAttributeOverrides();

            typeConverterFactories = new LazyComponentRegistrationList<Nothing, IYamlTypeConverter>
            {
                _ => new GuidConverter(false), _ => new SystemTypeConverter()
            };

            typeInspectorFactories = new LazyComponentRegistrationList<ITypeInspector, ITypeInspector>();
            this.typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
        }

        protected IEnumerable<IYamlTypeConverter> BuildTypeConverters()
        {
            return typeConverterFactories.BuildComponentList();
        }
    }
}
