using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityWebServiceReadOnly<TEntity, TInterface, TId, TService>
               : IEntityWebServiceReadOnly<TEntity, TId>
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
            return EntityType.ToCsdl<TEntity>();
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
        public virtual OdataObjectCollection<TEntity, TId> GetAll()
        {
            if (UrlParameters.Count > 0)
            {
                var entities = Service.Get(UrlParameters)?.ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(RequestUri);
                RelatedEntityFetcher.Fetch(entities, UrlParameters);
                return entities;
            }
            return Service.Get()?.ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(RequestUri);
        }

        /// <summary>
        /// Get a list of Entities where the Entity id is in the list of ids provided.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>A List{OdataObject{T}} of entities where each is wrapped in an Odata object.</returns>
        public virtual OdataObjectCollection<TEntity, TId> GetByIds(List<TId> ids)
        {
            var entities = Service.Get(ids)?.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(RequestUri);
            RelatedEntityFetcher.Fetch(entities, UrlParameters);
            return entities;
        }

        /// <summary>
        /// Get a list of Entities where the value of the property of a given Entity is in the list of values provided.
        /// </summary>
        /// <param name="collection">A ValueCollection that has the property name and the values as strings.</param>
        /// <returns>A List{OdataObject{T}} of entities where each is wrapped in an Odata object.</returns>
        public virtual OdataObjectCollection<TEntity, TId> GetByPropertyValues(string property, List<string> values)
        {
            #region parameter validation
            if (string.IsNullOrWhiteSpace(property))
                throw new ArgumentException($"The property section of the Url must be a valid property of {typeof(TEntity).FullName}", "property");
            if (values == null || !values.Any())
                return null;
            var propInfo = typeof(TEntity).GetProperty(property);
            if (propInfo == null)
                throw new ArgumentException($"ValueCollection.Property must be a valid property of {typeof(TEntity).FullName}");
            #endregion
            var entities = Service.Get(property, values)?.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(RequestUri);
            RelatedEntityFetcher.Fetch(entities, UrlParameters);
            return entities;
        }

        /// <summary>
        /// Get the exact entity with the id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>A OdataObject<T, TId> object contain the single entity with the id provided.</returns>
        public virtual OdataObject<TEntity, TId> Get(string id)
        {
            var entity = Service.Get(id.To<TId>())?.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(RequestUri);
            RelatedEntityFetcher.Fetch(new[] { entity }, UrlParameters);
            return entity;
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

        #region Properties
        public static Type EntityType => typeof(TEntity);
        protected internal virtual Uri RequestUri => WebOperationContext.Current?.IncomingRequest?.UriTemplateMatch?.RequestUri;
        protected internal virtual NameValueCollection UrlParameters => WebOperationContext.Current?.IncomingRequest?.UriTemplateMatch?.QueryParameters;
        #endregion

        #region Injectable Dependency
        protected virtual IServiceCommon<TEntity, TInterface, TId> Service
        {
            get { return _Service ?? (_Service = new EntityServiceLoader<TEntity, TService>().LoadPluginOrCommon()); }
            set { _Service = value; }
        } protected IServiceCommon<TEntity, TInterface, TId> _Service;

        public IRelatedEntityFetcher<TEntity, TId> RelatedEntityFetcher
        {
            get { return _RelatedEntityFetcher ?? (_RelatedEntityFetcher = new RelatedEntityFetcher<TEntity, TInterface, TId>()); }
            set { _RelatedEntityFetcher = value; }
        } private IRelatedEntityFetcher<TEntity, TId> _RelatedEntityFetcher;

        #endregion
    }
}