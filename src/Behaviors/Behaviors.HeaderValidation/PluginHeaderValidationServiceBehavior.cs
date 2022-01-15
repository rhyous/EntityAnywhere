using Autofac;
using Autofac.Integration.Wcf;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Behaviors.DependencyInjection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class PluginHeaderValidationServiceBehavior : ServiceBehaviorBase
    {
        public static ILifetimeScope MyScope;
        public override void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            MyScope = AutofacHostFactory.Container.BeginLifetimeScope($"{nameof(PluginHeaderValidationServiceBehavior)}", b =>
            {
                b.RegisterModule<PluginHeaderValidationModule>();
            });
            var inspector = MyScope.Resolve<IHeaderValidationInspector>();
            foreach (var dispatcher in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = dispatcher as ChannelDispatcher;
                if (channelDispatcher != null)
                {
                    foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
                    }
                }
            }
        }

        public override ServiceBehaviorType Type { get { return ServiceBehaviorType.Authenticator; } }
    }
}