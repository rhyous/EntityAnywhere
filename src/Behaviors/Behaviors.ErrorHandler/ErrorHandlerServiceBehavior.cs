using Rhyous.EntityAnywhere.Attributes;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class ErrorHandlerServiceBehavior : ServiceBehaviorBase
    {
        public override void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>())
            {
                channelDispatcher.ErrorHandlers.Add(new ErrorHandler(Logger));
            }
        }

        public override ServiceBehaviorType Type { get { return ServiceBehaviorType.ErrorHandler; } }
    }
}
