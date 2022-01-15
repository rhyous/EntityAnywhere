using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class EntityWebServiceReadOnly<TEntity, TInterface, TId>
               : IEntityWebServiceReadOnly<TEntity, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        protected WebOperationContext Context;

        private readonly IRestHandlerProviderReadOnly<TEntity, TInterface, TId> _RestHandlerProviderReadOnly;

        public EntityWebServiceReadOnly(IRestHandlerProviderReadOnly<TEntity, TInterface, TId> restHandlerProvider)
        {
            Context = WebOperationContext.Current;
            _RestHandlerProviderReadOnly = restHandlerProvider;
        }

        /// <summary>
        /// This retuns metadata about the services.
        /// </summary>
        /// <returns>Schema of entity. Should be in CSDL (option for both json or xml should exist)</returns>
        public virtual async Task<CsdlEntity> GetMetadataAsync()
            => await _RestHandlerProviderReadOnly.GetMetadataHandler.Handle(typeof(TEntity));

        /// <summary>
        /// Gets the number of total entities
        /// </summary>
        /// <returns>The number of total entities.</returns>
        public virtual async Task<int> GetCountAsync() => await _RestHandlerProviderReadOnly.GetCountHandler.HandleAsync();

        /// <summary>
        /// Gets all entities.
        /// Note: Be careful using this on entities that are extremely large in quantity.
        /// </summary>
        /// <returns>List{OdataObject{T}} containing all entities</returns>
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetAllAsync()
            => await _RestHandlerProviderReadOnly.GetAllHandler.HandleAsync();

        /// <summary>
        /// Get a list of Entities where the Entity id is in the list of ids provided.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>A List{OdataObject{T}} of entities where each is wrapped in an Odata object.</returns>
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByIdsAsync(List<TId> ids)
            => await _RestHandlerProviderReadOnly.GetByIdsHandler.HandleAsync(ids);

        /// <summary>
        /// Get a list of Entities where the value of the property of a given Entity is in the list of values provided.
        /// </summary>
        /// <param name="collection">A ValueCollection that has the property name and the values as strings.</param>
        /// <returns>A List{OdataObject{T}} of entities where each is wrapped in an Odata object.</returns>
        public virtual async Task<OdataObjectCollection<TEntity, TId>> GetByPropertyValuesAsync(string property, List<string> values)
            => await _RestHandlerProviderReadOnly.GetByPropertyValuesHandler.HandleAsync(property, values);

        /// <summary>
        /// Get the exact entity with the id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>A OdataObject<T, TId> object contain the single entity with the id provided.</returns>
        public virtual async Task<OdataObject<TEntity, TId>> GetAsync(string id)
            => await _RestHandlerProviderReadOnly.GetByIdHandler.HandleAsync(id);

        /// <summary>
        /// Gets the value of one of the entity properties. 
        /// Example: if the entity is User and you want to get only the value of User.Username, not 
        /// the whole entity.
        /// </summary>
        /// <param name="id">The Entity Id.</param>
        /// <param name="property">A primitive property of the entity id.</param>
        /// <returns>The value of the property.</returns>
        public virtual string GetProperty(string id, string property)
            => _RestHandlerProviderReadOnly.GetPropertyHandler.Handle(id, property);

        #region Properties
        public static Type EntityType => typeof(TEntity);
        #endregion

        #region IDisposable

        public void Dispose()
        {
        }

        #endregion
    }
}