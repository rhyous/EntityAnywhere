using Autofac;
using Autofac.Core;
using Rhyous.StringLibrary.Pluralization;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    public class EntityClientCommonModule : Module
    {
        private readonly IEntityList _EntityList;
        private readonly IMappingEntityList _MappingEntityList;

        public EntityClientCommonModule(IEntityList entityList, 
                                        IMappingEntityList mappingEntityList)
        {
            _EntityList = entityList;
            _MappingEntityList = mappingEntityList;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(HttpClientFactory.Instance)
                .As<IHttpClientFactory>()
                .SingleInstance();

            builder.RegisterType<HttpClientRunnerNoHeaders>()
                   .As<IHttpClientRunnerNoHeaders>()
                   .SingleInstance();

            // Entity Client base URL
            builder.RegisterType<EntityClientConfig>()
                   .As<IEntityClientConfig>()
                   .SingleInstance();

            // NamedFactory is used to create named EntityClientAsync objects.
            builder.RegisterGeneric(typeof(NamedFactory<>))
                   .As(typeof(INamedFactory<>));

            // EntityClientConnectionSettings
            builder.RegisterGeneric(typeof(EntityClientConnectionSettings<>))
                   .As(typeof(IEntityClientConnectionSettings<>))
                   .SingleInstance();

            foreach (var entity in _EntityList.EntityNames)
            {
                builder.RegisterType<EntityClientConnectionSettings>()
                       .Named<IEntityClientConnectionSettings>(entity)
                       .WithParameter("entityName", entity)
                       .SingleInstance();
            }

            // Mapping settings
            builder.RegisterGeneric(typeof(MappingEntitySettings<>))
                   .As(typeof(IMappingEntitySettings<>))
                   .SingleInstance();

            foreach (var entityType in _MappingEntityList.Entities)
            {
                var attribute = entityType.GetAttribute<MappingEntityAttribute>();
                if (attribute == null)
                    throw new MissingConfigurationException("A mapping entity class must be decorated with the [MappingEntity] attribute");
                builder.RegisterType<MappingEntitySettings>()
                       .Named<IMappingEntitySettings>(entityType.Name)
                       .WithParameter("entityName", entityType.Name)
                       .WithParameter("entity1", attribute.Entity1)
                       .WithParameter("entity1Pluralized", attribute.Entity1.Pluralize())
                       .WithParameter("entity1Property", attribute.Entity1MappingProperty)
                       .WithParameter("entity2", attribute.Entity2)
                       .WithParameter("entity2Pluralized", attribute.Entity2.Pluralize())
                       .WithParameter("entity2Property", attribute.Entity2MappingProperty)
                       .SingleInstance();
            }
        }
    }
}