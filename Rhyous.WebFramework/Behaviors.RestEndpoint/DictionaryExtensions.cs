using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> defaultValueProvider = null)
        {
            TValue value;
            if (defaultValueProvider == null)
            {
                var dictProvider = dictionary as IDictionaryDefaultValueProvider<TKey, TValue>;
                if (dictProvider != null)
                    defaultValueProvider = dictProvider.DefaultValueProvider;
            }
            return dictionary.TryGetValue(key, out value) || defaultValueProvider == null
                ? value
                : defaultValueProvider.Invoke(key);
        }
    }
}
