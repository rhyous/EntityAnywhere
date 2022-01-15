using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Behaviors;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.Behaviors.RestEndpoint.Tests
{
    internal class ServiceBehavior1 : ServiceBehaviorBase
    {
        public override void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
        public override ServiceBehaviorType Type => ServiceBehaviorType.Authenticator;
    }
}