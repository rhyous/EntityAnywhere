using Autofac;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.WebServices;
using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.WebApi
{
    class EntityControllerBuilder : IEntityControllerBuilder
    {
        private readonly IEntityActionNameAttributeUpdater _EntityActionNameAttributeUpdater;
        private readonly IWebApiScopeContainerBuilder _WebApiScopeContainerBuilder;
        private readonly IAttributeToServiceDictionary _AttributeToServiceDictionary;

        public EntityControllerBuilder(IEntityActionNameAttributeUpdater entityActionNameAttributeUpdater,
                                       IWebApiScopeContainerBuilder webApiScopeContainerBuilder,
                                       IAttributeToServiceDictionary attributeToServiceDictionary)
        {
            _EntityActionNameAttributeUpdater = entityActionNameAttributeUpdater;
            _WebApiScopeContainerBuilder = webApiScopeContainerBuilder;
            _AttributeToServiceDictionary = attributeToServiceDictionary;
        }

        public Type Build(Type entityType)
        {
            var typeName = entityType.Name;
            var interfaceName = "I" + entityType.Name;
            var idType = entityType.GetInterface(typeof(IBaseEntity<>).Name)?.GetGenericArguments().First();
            if (idType == null)
                throw new ConfigurationException($"The type {typeName} must implement IBaseEntity<TId>.");

            var interfaceType = entityType.GetInterface(interfaceName);
            if (interfaceType == null)
                throw new ConfigurationException($"The entity {typeName} must have an interface named {interfaceName}.");

            var attributes = entityType.GetCustomAttributes(true)
                                .Where(a => typeof(EntityAttribute).IsAssignableFrom(a.GetType())
                                         || a is MappingEntityAttribute);
            Type type = typeof(EntityController<,,>);
            foreach (var att in attributes)
            {
                if (_AttributeToServiceDictionary.TryGetValue(att.GetType(), out Type? foundType))
                {
                    type = foundType;
                    break;
                }
            }

            var idTypeAttribute = entityType.GetAttribute<IdTypeAttribute>(true);

            var additionalWebServiceTypes = entityType.GetAdditionalTypes<AdditionalWebServiceTypesAttribute>();
            var webServiceTypes = idTypeAttribute == null || idTypeAttribute.IsGenericForWebService
                                ? ArrayMaker.Make(entityType, interfaceType, idType, additionalWebServiceTypes)
                                : ArrayMaker.Make(entityType, interfaceType, additionalWebServiceTypes);
            var controllerType = type.MakeGenericType(webServiceTypes);
            _EntityActionNameAttributeUpdater.Update(controllerType, entityType);
            _WebApiScopeContainerBuilder.Register(controllerType);
            return controllerType;
        }
    }
}