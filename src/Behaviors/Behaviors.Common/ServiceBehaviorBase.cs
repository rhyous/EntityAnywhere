using Autofac;
using Autofac.Integration.Wcf;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public abstract class ServiceBehaviorBase : IServiceBehavior, ILogProperty
    {
        public virtual void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public abstract void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase);

        public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public virtual ServiceBehaviorType Type { get; }

        public ILogger Logger
        {
            get { return _Logger ?? (_Logger = AutofacHostFactory.Container.Resolve<ILogger>()); }
            set { _Logger = value; }
        } private ILogger _Logger;

    }
}
