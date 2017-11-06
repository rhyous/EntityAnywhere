using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityWebServiceReadOnly<TEntity, TInterface, TId, TService>
               : EntityWebServiceAddenda<TEntity>, IEntityWebServiceReadOnly<TEntity, TId>
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
        public virtual List<OdataObject<TEntity, TId>> GetAll()
        {
            if (UrlParameters.Count > 0)
                return Service.Get(UrlParameters)?.ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(RequestUri);
            return Service.Get()?.ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(RequestUri);
        }

        /// <summary>
        /// Get a list of Entities where the Entity id is in the list of ids provided.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>A List{OdataObject{T}} of entities where each is wrapped in an Odata object.</returns>
        public virtual List<OdataObject<TEntity, TId>> GetByIds(List<TId> ids)
        {
            var addendumlist = GetAddendaByEntityIds(ids.Select(id => id.ToString()).ToList());
            var entities = Service.Get(ids)?.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(RequestUri, addendumlist);
            var relatedEntities = Service.GetRelatedEntities(entities.Select(o => o.Object), UrlParameters);
            Sorter.Collate(entities, relatedEntities);
            return entities;
        }

        /// <summary>
        /// Get the exact entity with the id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>A OdataObject<T, TId> object contain the single entity with the id provided.</returns>
        public virtual OdataObject<TEntity, TId> Get(string id)
        {
            var entity = Service.Get(id.To<TId>())?.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(RequestUri, GetAddenda(id));
            entity.RelatedEntities = Service.GetRelatedEntities(entity.Object, UrlParameters);
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

        public IRelatedEntitySorter<TEntity, TId> Sorter
        {
            get { return _Sorter ?? (_Sorter = new RelatedEntitySorter<TEntity, TId>()); }
            set { _Sorter = value; }
        } private IRelatedEntitySorter<TEntity, TId> _Sorter;

        #endregion
    }
}
