using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// A common entity web service for entities that have the AlternateKey attribute, which indicates a second key field, such as Name.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TService">The entity service type.</typeparam>
    public class EntityWebServiceAlternateKey<TEntity, TInterface, TId, TAltKey> 
        : EntityWebService<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        protected readonly IRestHandlerProviderAlternateKey<TEntity, TInterface, TId, TAltKey> _RestHandlerProvider;

        public EntityWebServiceAlternateKey(IRestHandlerProviderAlternateKey<TEntity, TInterface, TId, TAltKey> restHandlerProvider)
            : base(restHandlerProvider)
        {
            _RestHandlerProvider = restHandlerProvider;
        }

        public override async Task<OdataObject<TEntity, TId>> GetAsync(string idOrAltId)
        {
            return await _RestHandlerProvider.GetByAlternateKeyHandler.HandleAsync(idOrAltId);
        }
    }
}