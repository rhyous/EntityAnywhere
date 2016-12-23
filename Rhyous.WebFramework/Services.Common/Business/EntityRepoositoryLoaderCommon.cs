using Rhyous.WebFramework.Interfaces;
using System.IO;

namespace Rhyous.WebFramework.Services
{
    public class EntityRepoositoryLoaderCommon<T, Tinterface, Tid> : PluginLoaderBase<IRepository<T, Tinterface, Tid>>
    {
        public override string PluginSubFolder => Path.Combine("Repositories", "Common");
    }

}