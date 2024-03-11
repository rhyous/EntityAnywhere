using Autofac;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.DependencyInjection
{
    public class EntityPropertyServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntityPropertyService>()
                   .As<IEntityPropertyService>()
                   .As<IServiceCommon<EntityProperty, IEntityProperty, int>>();
        }
    }
}