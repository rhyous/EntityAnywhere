using System;

namespace Rhyous.WebFramework.Common
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionaryDefaultValueProvider<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> defaultValueProvider = null)
        {
            TValue value;
            if (defaultValueProvider == null)
            {
                defaultValueProvider = dictionary.DefaultValueProvider;
            }
            return dictionary.TryGetValue(key, out value) || defaultValueProvider == null
                ? value
                : defaultValueProvider.Invoke(key);
        }
    }
}
