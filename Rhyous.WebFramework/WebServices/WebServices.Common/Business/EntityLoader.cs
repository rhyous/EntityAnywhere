using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.WebServices
{

    public class EntityLoader : PluginLoaderBase<IEntity>
    {
        public override string PluginSubFolder => "Entities";
    }

}