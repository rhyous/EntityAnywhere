using Rhyous.StringLibrary;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EntityWebService<TEntity, TInterface, TId, TService> : IEntityWebService<TEntity, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TService : class, IServiceCommon<TEntity, TInterface, TId>, new()
    {
        /// <summary>
        /// This retuns metadata about the services.
        /// </summary>
        /// <returns>Schema of entity. Should be in CSDL (option for both json or xml should exist)</returns>
        public virtual CsdlEntity<TEntity> GetMetadata()
        {
            var entity = new CsdlEntity<TEntity> { Keys = new List<string> { "Id" } };
            foreach (var property in EntityType.GetProperties())
            {
                var isNullable = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                var propertyType = isNullable ? property.PropertyType.GetGenericArguments()[0] : property.PropertyType;
                if (propertyType.FullName != null && CsdlTypeDictionary.Instance.ContainsKey(propertyType.FullName))
                {
                    var csdlTypes = new List<string> {CsdlTypeDictionary.Instance[propertyType.FullName]};
                    if (isNullable)
                        csdlTypes.Add("null");
                    entity.Properties.Add(new CsdlProperty { Name = property.Name, CsdlType = csdlTypes, CsdlFormat = CsdlFormatDictionary.Instance[propertyType.FullName] });
                }
            }
            return entity;
        }

        /// <summary>
        /// Gets the number of total entities
        /// </summary>
        /// <returns>The number of total entities.</returns>
        public virtual int GetCount()
        {
            return Service.GetCount();
        }

        /// <summary>
        /// Gets all entities.
        /// Note: Be careful using this on entities that are extremely large in quantity.
        /// </summary>
        /// <returns>List{OdataObject{T}} containing all entities</returns>
        public virtual List<OdataObject<TEntity>> GetAll()
        {
            if (WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters.Count > 0)
                return Service.Get(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters)?.ToConcrete<TEntity, TInterface>().ToList().AsOdata(RequestUri);
            return Service.Get()?.ToConcrete<TEntity, TInterface>().ToList().AsOdata(RequestUri);
        }

        /// <summary>
        /// Get a list of Entities where the Entity id is in the list of ids provided.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>A List{OdataObject{T}} of entities where each is wrapped in an Odata object.</returns>
        public virtual List<OdataObject<TEntity>> GetByIds(List<TId> ids)
        {
            return Service.Get(ids)?.ToConcrete<TEntity, TInterface>().ToList().AsOdata(RequestUri);
        }

        /// <summary>
        /// Get the exact entity with the id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>A OdataObject<T> object contain the single entity with the id provided.</returns>
        public virtual OdataObject<TEntity> Get(string id)
        {
            return Service.Get(id.To<TId>())?.ToConcrete<TEntity, TInterface>().AsOdata(RequestUri, GetAddenda(id));
        }

        /// <summary>
        /// Gets the value of one of the entity properties. 
        /// Example: if the entity is User and you want to get only the value of User.Username, not 
        /// the whole entity.
        /// </summary>
        /// <param name="id">The Entity Id.</param>
        /// <param name="property">A primitive property of the entity id.</param>
        /// <returns>The value of the property.</returns>
        public virtual string GetProperty(string id, string property)
        {
            return Service.GetProperty(id.To<TId>(), property);
        }

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
        public virtual List<OdataObject<TEntity>> Post(List<TEntity> entities)
        {
            return Service.Add(entities.ToList<TInterface>()).ToConcrete<TEntity, TInterface>().ToList().AsOdata(RequestUri);
        }

        /// <summary>
        /// Updates one ore more properties of an entity.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="entity">The current entity or a stub entity where at minimum the properties set are
        /// the required properties for deserialization and changed properties.</param>
        /// <param name="changedProperties">the list of properties that are being changed.</param>
        /// <returns>The changed entity.</returns>
        public virtual OdataObject<TEntity> Patch(string id, PatchedEntity<TEntity> patchedEntity)
        {
            return Service.Update(id.To<TId>(), patchedEntity.Entity, patchedEntity.ChangedProperties).ToConcrete<TEntity, TInterface>().AsOdata(RequestUri, GetAddenda(id));
        }

        /// <summary>
        /// Replace the entity stored with the provided entity.
        /// </summary>
        /// <param name="id">The entity id to replace.</param>
        /// <param name="entity">The new entity.</param>
        /// <returns>The new entity.</returns>
        public virtual OdataObject<TEntity> Put(string id, TEntity entity)
        {
            return Service.Replace(id.To<TId>(), entity).ToConcrete<TEntity, TInterface>().AsOdata(RequestUri, GetAddenda(id));
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

        public virtual List<Addendum> GetAddenda(string id)
        {
            var entityName = typeof(TEntity).Name;
            return AddendaService.Get(x => x.Entity == entityName && x.EntityId == id.ToString())
                                 .ToConcrete<Addendum>().ToList();
        }

        public virtual List<Addendum> GetAddendaByEntityIds(List<string> entityIds)
        {
            var entityName = typeof(TEntity).Name;
            return AddendaService.Get(x => x.Entity == entityName && entityIds.Contains(x.EntityId))
                                 .ToConcrete<Addendum>().ToList();
        }

        /// <summary>
        /// Gets and addendum value for the entity id by the addendum name.
        /// </summary>
        /// <param name="id">The Entity Id.</param>
        /// <param name="name">The property name of the addendum.</param>
        /// <returns>The value of the addendum.</returns>
        public virtual Addendum GetAddendaByName(string id, string name)
        {
            var entityName = typeof(TEntity).Name;
            return AddendaService.Get(x => x.Entity == entityName && x.EntityId == id.ToString() && x.Property.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                 .OrderByDescending(x => x.CreateDate)
                                 .FirstOrDefault()
                                 .ToConcrete<Addendum>();
        }

        protected internal virtual Uri RequestUri
        {
            get { return WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri; }
        }

        #region Type property
        public static Type EntityType => typeof(TEntity);
        #endregion

        #region Injectable Dependency
        protected virtual IServiceCommon<TEntity, TInterface, TId> Service
        {
            get { return _Service ?? (_Service = new EntityServiceLoader<TEntity, TService>().LoadPluginOrCommon()); }
            set { _Service = value; }
        } protected IServiceCommon<TEntity, TInterface, TId> _Service;

        protected virtual IServiceCommon<Addendum, IAddendum, long> AddendaService
        {
            get { return _AddendaService ?? (_AddendaService = new ServiceCommon<Addendum, IAddendum, long>()); }
            set { _AddendaService = value; }
        } private IServiceCommon<Addendum, IAddendum, long> _AddendaService;
        #endregion

    }
}
