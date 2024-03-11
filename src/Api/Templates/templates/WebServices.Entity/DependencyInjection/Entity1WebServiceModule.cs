using Autofac;

namespace Rhyous.EntityAnywhere.WebServices.DependencyInjection
{
    public class Entity1WebServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<YourType>().As<IYourType>();
        }
    }
}