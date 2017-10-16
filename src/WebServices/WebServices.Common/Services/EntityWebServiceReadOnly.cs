using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var addendumlist = GetAddendaByEntityIds(ids.Select(id => id.ToString()).ToList());
            return Service.Get(ids)?.ToConcrete<TEntity, TInterface>().AsOdata<TEntity, TId>(RequestUri, addendumlist);
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


        #region Properties
        public static Type EntityType => typeof(TEntity);
        protected internal virtual Uri RequestUri
        {
            get { return WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri; }
        }
        #endregion

        #region Injectable Dependency
        protected virtual IServiceCommon<TEntity, TInterface, TId> Service
        {
            get { return _Service ?? (_Service = new EntityServiceLoader<TEntity, TService>().LoadPluginOrCommon()); }
            set { _Service = value; }
        } protected IServiceCommon<TEntity, TInterface, TId> _Service;        
        #endregion

    }
}
