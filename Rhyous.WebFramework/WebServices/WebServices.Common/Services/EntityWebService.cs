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
        where Tid : struct, IComparable, IComparable<Tid>, IEquatable<Tid>
        where TService : class, IServiceCommon<T, Tinterface, Tid>, new()
    {
        public virtual EntityMetadata<T> GetMetadata()
        {
            return new EntityMetadata<T>() { EntityName = typeof(T).Name, ExampleEntity = new T() };
        }

        public virtual List<OdataObject<T>> GetAll()
        {
            return Service.Get()?.ToConcrete<T, Tinterface>().ToList().AsOdata(GetRequestUri());
        }

        public virtual List<OdataObject<T>> GetByIds(List<Tid> ids)
        {
            return Service.Get(ids)?.ToConcrete<T, Tinterface>().ToList().AsOdata(GetRequestUri());
        }

        public virtual OdataObject<T> Get(string id)
        {
            return Service.Get(id.To<Tid>())?.ToConcrete<T, Tinterface>().AsOdata(GetRequestUri());
        }

        public virtual string GetProperty(string id, string property)
        {
            return Service.GetProperty(id.To<Tid>(), property);
        }

        public virtual List<T> Post(List<T> entities)
        {
            return Service.Add(entities.ToList<Tinterface>()).ToConcrete<T, Tinterface>().ToList();
        }

        public virtual T Patch(string id, T entity, List<string> changedProperties)
        {
            return Service.Update(id.To<Tid>(), entity, changedProperties).ToConcrete<T, Tinterface>();
        }

        public virtual T Put(string id, T entity)
        {
            return Service.Replace(id.To<Tid>(), entity).ToConcrete<T, Tinterface>();
        }

        public virtual bool Delete(string id)
        {
            return Service.Delete(id.To<Tid>());
        }

        public virtual Uri GetRequestUri()
        {
            return WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri;
        }

        public virtual List<Addendum> GetAddenda(string id)
        {
            var entityName = typeof(T).Name;
            return AddendaService.Get(x => x.Entity == entityName && x.EntityId == id.ToString())
                                 .ToConcrete<Addendum>().ToList();
        }

        public virtual Addendum GetAddendaByName(string id, string name)
        {
            var entityName = typeof(T).Name;
            return AddendaService.Get(x => x.Entity == entityName && x.EntityId == id.ToString())
                                 .OrderByDescending(x => x.CreateDate)
                                 .FirstOrDefault()
                                 .ToConcrete<Addendum>();
        }

        #region Type property
        public static Type EntityType => typeof(T);
        #endregion

        #region Injectable Dependency
        protected virtual IServiceCommon<T, Tinterface, Tid> Service
        {
            get { return _Service ?? (_Service = new EntityServiceLoader<T, Tinterface, Tid>().LoadPluginOrCommon()); }
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
