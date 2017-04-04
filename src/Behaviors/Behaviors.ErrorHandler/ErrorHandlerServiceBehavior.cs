using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.WebServices;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Rhyous.WebFramework.Behaviors
{
    public class ErrorHandlerServiceBehavior : ServiceBehaviorBase
    {
        public override void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>())
            {
                channelDispatcher.ErrorHandlers.Add(new ErrorHandler());
            }
        }

        public override ServiceBehaviorType Type { get { return ServiceBehaviorType.ErrorHandler; } }
    }
}
