using Autofac;

namespace Rhyous.EntityAnywhere.Services.DependencyInjection
{
    public class Entity1ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<YourType>().As<IYourType>();
        }
    }
}