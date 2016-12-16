namespace Rhyous.WebFramework.Behaviors
{
    public interface IDictionaryDefaultValueProvider<TKey, TValue>
    {
        TValue DefaultValueProvider(TKey key);
    }
}
