using System;
using System.Collections;
using System.ComponentModel;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization.ObjectGraphVisitors
{
    public sealed class DefaultValuesObjectGraphVisitor : ChainedObjectGraphVisitor
    {
        private readonly DefaultValuesHandling handling;
        private readonly IObjectFactory factory;

        public DefaultValuesObjectGraphVisitor(DefaultValuesHandling handling, IObjectGraphVisitor<IEmitter> nextVisitor, IObjectFactory factory)
            : base(nextVisitor)
        {
            this.handling = handling;
            this.factory = factory;
        }

        private object? GetDefault(Type type) => factory.CreatePrimitive(type);

        public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
        {
            var configuration = handling;

            if ((configuration & DefaultValuesHandling.OmitNull) != 0)
            {
                if (value.Value is null)
                {
                    return false;
                }
            }

            if ((configuration & DefaultValuesHandling.OmitEmptyCollections) != 0)
            {
                if (value.Value is IEnumerable enumerable)
                {
                    var enumerator = enumerable.GetEnumerator();
                    var canMoveNext = enumerator.MoveNext();
                    if (enumerator is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }

                    if (!canMoveNext)
                    {
                        return false;
                    }
                }
            }

            if ((configuration & DefaultValuesHandling.OmitDefaults) != 0)
            {
                var defaultValue = key.GetCustomAttribute<DefaultValueAttribute>()?.Value ?? GetDefault(key.Type);
                if (Equals(value.Value, defaultValue))
                {
                    return false;
                }
            }

            return base.EnterMapping(key, value, context);
        }
    }
}
