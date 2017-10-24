using Rhyous.StringLibrary;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// A common entity web service for all entities. If no custom entity web service is provided, this one is used.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The entity id type.</typeparam>
    /// <typeparam name="TService">The entity service type.</typeparam>
    public class EntityWebService<TEntity, TInterface, TId, TService>
               : EntityWebServiceReadOnly<TEntity, TInterface, TId, TService>, IEntityWebService<TEntity, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TService : class, IServiceCommon<TEntity, TInterface, TId>, new()
    {
        /// <summary>
        /// Updates a single properties value
        /// </summary>
        /// <param name="id">The entity's Id.</param>
        /// <param name="property">The property name to update.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string UpdateProperty(string id, string property, string value)
        {
            return Service.UpdateProperty(id.To<TId>(), property, value);
        }

        /// <summary>
        /// Creates one or more (an array) of entities. 
        /// </summary>
        /// <param name="entities">The list of entities to create</param>
        /// <returns>The created entities.</returns>
        public virtual List<OdataObject<TEntity, TId>> Post(List<TEntity> entities)
        {
            return Service.Add(entities.ToList<TInterface>()).ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(RequestUri);
        }

        /// <summary>
        /// Updates one ore more properties of an entity.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="entity">The current entity or a stub entity where at minimum the properties set are
        /// the required properties for deserialization and changed properties.</param>
        /// <param name="changedProperties">the list of properties that are being changed.</param>
        /// <returns>The changed entity.</returns>
        public virtual OdataObject<TEntity, TId> Patch(string id, PatchedEntity<TEntity> patchedEntity)
        {
            return Service.Update(id.To<TId>(), patchedEntity.Entity, patchedEntity.ChangedProperties).ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(RequestUri, GetAddenda(id));
        }

        /// <summary>
        /// Replace the entity stored with the provided entity.
        /// </summary>
        /// <param name="id">The entity id to replace.</param>
        /// <param name="entity">The new entity.</param>
        /// <returns>The new entity.</returns>
        public virtual OdataObject<TEntity, TId> Put(string id, TEntity entity)
        {
            return Service.Replace(id.To<TId>(), entity).ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(RequestUri, GetAddenda(id));
        }


        /// <summary>
        /// Deletes the entity at the given id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>true if the entity could be deleted, false otherwise.</returns>
        public virtual bool Delete(string id)
        {
            return Service.Delete(id.To<TId>());
        }

    }
}
