using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyExceptions.Yaml.Serialization.ObjectFactories
{
    /// <summary>
    /// Creates objects using Activator.CreateInstance.
    /// </summary>
    public sealed class DefaultObjectFactory : ObjectFactoryBase
    {
        private readonly Dictionary<Type, Type> DefaultGenericInterfaceImplementations = new Dictionary<Type, Type>
        {
            { typeof(IEnumerable<>), typeof(List<>) },
            { typeof(ICollection<>), typeof(List<>) },
            { typeof(IList<>), typeof(List<>) },
            { typeof(IDictionary<,>), typeof(Dictionary<,>) }
        };

        private readonly Dictionary<Type, Type> DefaultNonGenericInterfaceImplementations = new Dictionary<Type, Type>
        {
            { typeof(IEnumerable), typeof(List<object>) },
            { typeof(ICollection), typeof(List<object>) },
            { typeof(IList), typeof(List<object>) },
            { typeof(IDictionary), typeof(Dictionary<object, object>) }
        };

        private readonly Settings settings;

        public DefaultObjectFactory()
            : this(new Dictionary<Type, Type>(), new Settings())
        {
        }

        public DefaultObjectFactory(IDictionary<Type, Type> mappings, Settings settings)
        {
            foreach (var pair in mappings)
            {
                if (!pair.Key.IsAssignableFrom(pair.Value))
                {
                    throw new InvalidOperationException($"Type '{pair.Value}' does not implement type '{pair.Key}'.");
                }

                DefaultNonGenericInterfaceImplementations.Add(pair.Key, pair.Value);
            }

            this.settings = settings;
        }

        public override object Create(Type type)
        {
            if (type.IsInterface())
            {
                if (type.IsGenericType())
                {
                    if (DefaultGenericInterfaceImplementations.TryGetValue(type.GetGenericTypeDefinition(), out var implementationType))
                    {
                        type = implementationType.MakeGenericType(type.GetGenericArguments());
                    }
                }
                else
                {
                    if (DefaultNonGenericInterfaceImplementations.TryGetValue(type, out var implementationType))
                    {
                        type = implementationType;
                    }
                }
            }

            try
            {
                return Activator.CreateInstance(type, settings.AllowPrivateConstructors)!;
            }
            catch (Exception err)
            {
                var message = $"Failed to create an instance of type '{type.FullName}'.";
                throw new InvalidOperationException(message, err);
            }
        }
    }
}
