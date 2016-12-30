using System;
using System.ServiceModel.Configuration;

namespace Rhyous.WebFramework.Behaviors
{
    public class RestEndpointBehaviorExtension : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof(RestEndpointBehavior);

        protected override object CreateBehavior()
        {
            return new RestEndpointBehavior();
        }
    }
}
