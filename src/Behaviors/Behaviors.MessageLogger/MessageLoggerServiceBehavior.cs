using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class MessageLoggerServiceBehavior : ServiceBehaviorBase
    {
        public override void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var dispatcher in serviceHostBase.ChannelDispatchers)
            {
                if (dispatcher is ChannelDispatcher channelDispatcher)
                {
                    foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new MessageLoggerInspector(Logger, new MessageHandler(), new UrlFilter()));
                    }
                }
            }
        }
    }
}