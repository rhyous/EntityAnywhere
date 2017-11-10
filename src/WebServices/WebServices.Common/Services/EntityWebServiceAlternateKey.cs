using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// A common entity web service for entities that have the AlternateKey attribute, which indicates a second key field that is a string, such as Name.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TService">The entity service type.</typeparam>
    public class EntityWebServiceAlternateKey<TEntity, TInterface, TId, TService> : EntityWebService<TEntity, TInterface, TId, TService>
        where TEntity : class, TInterface, new()
        where TInterface : IEntity<TId>
        where TService : class, IServiceCommonAlternateKey<TEntity, TInterface, TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public override OdataObject<TEntity, TId> Get(string idOrName)
        {
            if (string.IsNullOrWhiteSpace(idOrName))
                return null;
            var id = idOrName.To<TId>();
            if (id.Equals(default(TId)))
            {
                var entity = AltKeyService.Get(idOrName)?.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(RequestUri);
                Service.GetRelatedEntities(entity.Object);
                return entity;
            }
            return base.Get(idOrName);
        }
        
        public virtual IServiceCommonAlternateKey<TEntity, TInterface, TId> AltKeyService
        {
            get { return Service as IServiceCommonAlternateKey<TEntity, TInterface, TId>; }
        }
    }
}
