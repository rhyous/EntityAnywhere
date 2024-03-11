using Autofac;

namespace Rhyous.EntityAnywhere.WebServices.DependencyInjection
{
    public class Custom1WebServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<YourType>().As<IYourType>();
        }
    }
}