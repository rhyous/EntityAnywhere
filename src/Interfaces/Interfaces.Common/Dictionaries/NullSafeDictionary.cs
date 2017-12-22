using System;
using System.Collections;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// This class wraps a dictionary, but if a key that does not exist is provided to the indexer,
    /// then instead of an exception, Activate.CreateInstance() is returned.
    /// If you need to instantiate the object differently, inherit this class and override the DefaultValueProvider method.
    /// </summary>
    public class NullSafeDictionary<TKey, TValue> : IDictionaryDefaultValueProvider<TKey, TValue>
    {
        internal IDictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();

        public NullSafeDictionary() { }
        public NullSafeDictionary(Func<TKey, TValue> defaultValueProviderMethod) { _DefaultValueProviderMethod = defaultValueProviderMethod; }

        /// <summary>
        /// This indexer does not throw an System.ArgumentNullException, but instead instantiates the object with the default empty constructor.
        /// If you need to instantiate the object differently, replace the DefaultValueProviderMethod or inherit and override.
        /// </summary>
        /// <param name="key">The key to access</param>
        /// <returns>The TValue at the TKey position, otherwise an object instantiated via the default empty constructor.</returns>
        public virtual TValue this[TKey key]
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

        /// <summary>
        /// Adds a list of Keys and assigns them each the default value.
        /// </summary>
        /// <param name="keys"></param>
        public virtual void AddKeys(IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                Add(key, DefaultValueProvider(key));
            }
        }

        public virtual TValue DefaultValueProvider(TKey key)
        {
            return DefaultValueProviderMethod(key);
        }

        public virtual Func<TKey, TValue> DefaultValueProviderMethod
        {
            get { return _DefaultValueProviderMethod ?? (_DefaultValueProviderMethod => (TValue)Activator.CreateInstance(typeof(TValue))); }
            set { _DefaultValueProviderMethod = value; }
        }
        private Func<TKey, TValue> _DefaultValueProviderMethod;

        #region Interface

        public virtual ICollection<TKey> Keys => _Dictionary.Keys;

        public virtual ICollection<TValue> Values => _Dictionary.Values;

        public virtual int Count => _Dictionary.Count;

        public virtual bool IsReadOnly => _Dictionary.IsReadOnly;

        public virtual TValue DefaultValue => default(TValue);

        public virtual bool ContainsKey(TKey key)
        {
            return _Dictionary.ContainsKey(key);
        }

        public virtual void Add(TKey key, TValue value)
        {
            _Dictionary.Add(key, value);
        }

        public virtual bool Remove(TKey key)
        {
            return _Dictionary.Remove(key);
        }

        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            return _Dictionary.TryGetValue(key, out value);
        }

        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            _Dictionary.Add(item);
        }

        public virtual void Clear()
        {
            _Dictionary.Clear();
        }

        public virtual bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _Dictionary.Contains(item);
        }

        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _Dictionary.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _Dictionary.Remove(item);
        }

        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Dictionary.GetEnumerator();
        }
        #endregion
    }
}