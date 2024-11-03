using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyExceptions.Yaml.Helpers
{
    /// <summary>
    /// Adapts an <see cref="System.Collections.Generic.IDictionary{TKey, TValue}" /> to <see cref="IDictionary" />
    /// because not all generic dictionaries implement <see cref="IDictionary" />.
    /// </summary>
    internal sealed class GenericDictionaryToNonGenericAdapter<TKey, TValue> : IDictionary
        where TKey : notnull
    {
        private readonly IDictionary<TKey, TValue> genericDictionary;

        public GenericDictionaryToNonGenericAdapter(IDictionary<TKey, TValue> genericDictionary)
        {
            this.genericDictionary = genericDictionary ?? throw new ArgumentNullException(nameof(genericDictionary));
        }

        public void Add(object key, object? value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object key)
        {
            throw new NotSupportedException();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return new DictionaryEnumerator(genericDictionary.GetEnumerator());
        }

        public bool IsFixedSize => throw new NotSupportedException();

        public bool IsReadOnly => throw new NotSupportedException();

        public ICollection Keys => throw new NotSupportedException();

        public void Remove(object key)
        {
            throw new NotSupportedException();
        }

        public ICollection Values => throw new NotSupportedException();

        public object? this[object key]
        {
            get => throw new NotSupportedException();
            set => genericDictionary[(TKey)key] = (TValue)value!;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }

        public int Count => throw new NotSupportedException();

        public bool IsSynchronized => throw new NotSupportedException();

        public object SyncRoot => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
            {
                this.enumerator = enumerator;
            }

            public DictionaryEntry Entry => new DictionaryEntry(Key, Value);

            public object Key => enumerator.Current.Key!;

            public object? Value => enumerator.Current.Value;

            public object Current => Entry;

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Reset();
            }
        }
    }
}
