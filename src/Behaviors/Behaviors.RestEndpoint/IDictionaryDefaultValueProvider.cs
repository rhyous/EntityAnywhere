using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    public interface IDictionaryDefaultValueProvider<TKey, TValue> : IDictionary<TKey, TValue>
    {
        TValue DefaultValueProvider(TKey key);
    }
}
