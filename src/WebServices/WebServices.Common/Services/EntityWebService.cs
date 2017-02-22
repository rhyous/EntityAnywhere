using Rhyous.StringLibrary;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityWebService<T, Tinterface, Tid, TService> : IEntityWebService<T, Tid>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where TService : class, IServiceCommon<T, Tinterface, Tid>, new()
    {
        /// <summary>
        /// This retuns metadata about the services.
        /// </summary>
        /// <returns></returns>
        public virtual EntityMetadata<T> GetMetadata()
        {
            return new EntityMetadata<T>() { EntityName = typeof(T).Name, ExampleEntity = new T() };
        }

        /// <summary>
        /// Gets all entities.
        /// Note: Be careful using this on entities that are extremely large in quantity.
        /// </summary>
        /// <returns></returns>
        public virtual List<OdataObject<T>> GetAll()
        {
            return Service.Get()?.ToConcrete<T, Tinterface>().ToList().AsOdata(RequestUri);
        }

        /// <summary>
        /// Get a list of Entities where the Entity id is in the list of ids provided.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>A list of entities where each is wrapped in an Odata object.</returns>
        public virtual List<OdataObject<T>> GetByIds(List<Tid> ids)
        {
            return Service.Get(ids)?.ToConcrete<T, Tinterface>().ToList().AsOdata(RequestUri);
        }

        /// <summary>
        /// Get the exact entity with the id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>A single entity wrapped in an odata object.</returns>
        public virtual OdataObject<T> Get(string id)
        {
            return Service.Get(id.To<Tid>())?.ToConcrete<T, Tinterface>().AsOdata(RequestUri, GetAddenda(id));
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
            return Service.GetProperty(id.To<Tid>(), property);
        }

        /// <summary>
        /// Creates one or more (an array) of entities. 
        /// </summary>
        /// <param name="entities">The list of entities to create</param>
        /// <returns>The created entities.</returns>
        public virtual List<T> Post(List<T> entities)
        {
            return Service.Add(entities.ToList<Tinterface>()).ToConcrete<T, Tinterface>().ToList();
        }

        /// <summary>
        /// Updates one ore more properties of an entity.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="entity">The current entity or a stub entity where at minimum the properties set are
        /// the required properties for deserialization and changed properties.</param>
        /// <param name="changedProperties">the list of properties that are being changed.</param>
        /// <returns>The changed entity.</returns>
        public virtual T Patch(string id, T entity, List<string> changedProperties)
        {
            return Service.Update(id.To<Tid>(), entity, changedProperties).ToConcrete<T, Tinterface>();
        }

        /// <summary>
        /// Replace the entity stored with the provided entity.
        /// </summary>
        /// <param name="id">The entity id to replace.</param>
        /// <param name="entity">The new entity.</param>
        /// <returns>The new entity.</returns>
        public virtual T Put(string id, T entity)
        {
            return Service.Replace(id.To<Tid>(), entity).ToConcrete<T, Tinterface>();
        }


        /// <summary>
        /// Deletes the entity at the given id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>true if the entity could be deleted, false otherwise.</returns>
        public virtual bool Delete(string id)
        {
            return Service.Delete(id.To<Tid>());
        }

        public virtual List<Addendum> GetAddenda(string id)
        {
            var entityName = typeof(T).Name;
            return AddendaService.Get(x => x.Entity == entityName && x.EntityId == id.ToString())
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
            var entityName = typeof(T).Name;
            return AddendaService.Get(x => x.Entity == entityName && x.EntityId == id.ToString())
                                 .OrderByDescending(x => x.CreateDate)
                                 .FirstOrDefault()
                                 .ToConcrete<Addendum>();
        }

        protected internal virtual Uri RequestUri
        {
            get { return WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri; }
        }

        #region Type property
        public static Type EntityType => typeof(T);
        #endregion

        #region Injectable Dependency
        protected virtual IServiceCommon<T, Tinterface, Tid> Service
        {
            get { return _Service ?? (_Service = new EntityServiceLoader<T, TService>().LoadPluginOrCommon()); }
            set { _Service = value; }
        } protected IServiceCommon<T, Tinterface, Tid> _Service;

        protected virtual IServiceCommon<Addendum, IAddendum, long> AddendaService
        {
            get { return _AddendaService ?? (_AddendaService = new ServiceCommon<Addendum, IAddendum, long>()); }
            set { _AddendaService = value; }
        } private IServiceCommon<Addendum, IAddendum, long> _AddendaService;
        #endregion

    }
}
