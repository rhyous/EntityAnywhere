using Rhyous.WebFramework.Services;
using System.ServiceModel.Description;

namespace Rhyous.WebFramework.Behaviors
{

    public class ServiceBehaviorLoader : PluginLoaderBase<IServiceBehavior>
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => "ServiceBehaviors";
    }

}