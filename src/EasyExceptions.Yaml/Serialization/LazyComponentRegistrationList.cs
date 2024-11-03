using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasyExceptions.Yaml.Serialization
{
    internal sealed class LazyComponentRegistrationList<TArgument, TComponent> : IEnumerable<Func<TArgument, TComponent>>
    {
        private readonly List<LazyComponentRegistration> entries = new List<LazyComponentRegistration>();

        public LazyComponentRegistrationList<TArgument, TComponent> Clone()
        {
            var clone = new LazyComponentRegistrationList<TArgument, TComponent>();
            foreach (var entry in entries)
            {
                clone.entries.Add(entry);
            }
            return clone;
        }

        private sealed class LazyComponentRegistration
        {
            public readonly Func<TArgument, TComponent> Factory;

            public LazyComponentRegistration(Func<TArgument, TComponent> factory)
            {
                Factory = factory;
            }
        }

        public void Add(Func<TArgument, TComponent> factory)
        {
            entries.Add(new LazyComponentRegistration(factory));
        }

        public IEnumerable<Func<TArgument, TComponent>> InReverseOrder
        {
            get
            {
                for (var i = entries.Count - 1; i >= 0; --i)
                {
                    yield return entries[i].Factory;
                }
            }
        }

        public IEnumerator<Func<TArgument, TComponent>> GetEnumerator()
        {
            return entries.Select(e => e.Factory).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
