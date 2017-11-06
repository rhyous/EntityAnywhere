using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Interfaces
{
    public interface ISharedInterfaceDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        T GetValueOrNew<T>(TKey key) where T : class, TValue, new();
        T GetValueOrNew<T, TInput>(TKey key, TInput input1, Func<TInput, T> constructor = null) where T : class, TValue, new();
        T GetValueOrNew<T, T1Input, T2Input>(TKey key, T1Input input1, T2Input input2, Func<T1Input, T2Input, T> constructor = null) where T : class, TValue, new();
    }
}
