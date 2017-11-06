using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// This class wraps a dictionary, but if a key that does not exist is provided to the indexer,
    /// then instead of an exception, Activate.CreateInstance() is returned.
    /// If you need to instantiate the object differently, inherit this class and override the DefaultValueProvider method.
    /// </summary>
    public class NullSafeDictionary<TKey, TValue> : IDictionaryDefaultValueProvider<TKey, TValue>
    {
        internal IDictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();

        public ICollection<TKey> Keys => _Dictionary.Keys;

        public ICollection<TValue> Values => _Dictionary.Values;

        public int Count => _Dictionary.Count;

        public bool IsReadOnly => _Dictionary.IsReadOnly;

        public TValue DefaultValue => default(TValue);

        /// <summary>
        /// This indexer does not throw an System.ArgumentNullException, but instead instantiates the object with the default empty constructor.
        /// If you need to instantiate the object differently, inherit this class and override the DefaultValueProvider method.
        /// </summary>
        /// <param name="key">The key to access</param>
        /// <returns>The TValue at the TKey position, otherwise an object instantiated via the default empty constructor.</returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue client;
                if (_Dictionary.TryGetValue(key, out client))
                    return client;
                return _Dictionary[key] = DefaultValueProvider(key);
            }
            set { _Dictionary[key] = value; }
        }

        public bool ContainsKey(TKey key)
        {
            return _Dictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _Dictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return _Dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _Dictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _Dictionary.Add(item);
        }

        public void Clear()
        {
            _Dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _Dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _Dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _Dictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Dictionary.GetEnumerator();
        }

        public virtual TValue DefaultValueProvider(TKey key)
        {
            return this[key] = (TValue)Activator.CreateInstance(typeof(TValue));
        }
    }
}