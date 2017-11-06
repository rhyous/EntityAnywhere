using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Clients
{
    public interface IEntityClientCache
    {
        ISharedInterfaceDictionary<string, IEntityClientBase> Generic { get; set; }
        IDictionaryDefaultValueProvider<string, IEntityClientAsync> Json { get; set; }
    }
}