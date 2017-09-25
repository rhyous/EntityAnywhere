using System.Collections.Generic;

namespace Rhyous.WebFramework.Common
{
    public interface IDictionaryDefaultValueProvider<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// This method is used to provide a value when the Key is not found in the dictionary. 
        /// </summary>
        /// <param name="key">The key to find.</param>
        /// <returns>A default value.</returns>
        TValue DefaultValueProvider(TKey key);

        /// <summary>
        /// If all default values are the same, specify it here. Then the DefaultValueProvider can be just:
        /// return DefaultValue;
        /// If the default values are not all the same, do not use this field. You can hide this field, unless the object is cast to the interface, by implmenting this field explicity:
        ///     string IDictionaryDefaultValueProvider{string, string}.DefaultValue => null;
        /// </summary>
        TValue DefaultValue { get; }
    }
}
