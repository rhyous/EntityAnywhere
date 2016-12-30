using Rhyous.WebFramework.WebServices;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Rhyous.WebFramework.Behaviors
{
    public class PluginValidationServiceBehavior : ServiceBehaviorBase
    {
        public override void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var dispatcher in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = dispatcher as ChannelDispatcher;
                if (channelDispatcher != null)
                {
                    foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new HeaderValidationInspector());
                    }
                }
            }
        }
    }
}
