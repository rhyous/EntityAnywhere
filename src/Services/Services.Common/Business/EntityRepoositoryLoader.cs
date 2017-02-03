using Rhyous.WebFramework.Interfaces;
using System.IO;

namespace Rhyous.WebFramework.Services
{

    public class EntityRepoositoryLoader<T, Tinterface, Tid> : PluginLoaderBase<IRepository<T, Tinterface, Tid>>
        where T : Tinterface
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => Path.Combine("Repositories", typeof(T).Name);
    }

}