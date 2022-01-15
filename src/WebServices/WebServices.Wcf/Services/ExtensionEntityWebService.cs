using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class ExtensionEntityWebService<TEntity, TInterface>
                 : EntityWebService<TEntity, TInterface, long>,
                   IExtensionEntityWebService<TEntity>
        where TEntity : class, TInterface, new()
        where TInterface : IExtensionEntity, IBaseEntity<long>
    {
        private readonly IRestHandlerProvider<TEntity, TInterface, long> _RestHandlerProvider;

        public ExtensionEntityWebService(IRestHandlerProvider<TEntity, TInterface, long> restHandlerProvider)
            : base (restHandlerProvider)
        {
            _RestHandlerProvider = restHandlerProvider;
        }

        public OdataObjectCollection<TEntity, long> GetByEntityIdentifiers(List<EntityIdentifier> entityIdentifiers) 
                   => _RestHandlerProvider.GetByEntityIdentifiersHandler.Handle(entityIdentifiers);
        public OdataObjectCollection<TEntity, long> GetByEntityIds(string entity, List<string> ids)
                   => _RestHandlerProvider.GetByEntityIdentifiersHandler.Handle(entity, ids);
    }
}