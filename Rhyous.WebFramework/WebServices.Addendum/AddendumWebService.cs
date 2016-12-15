using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.Addendum;
using IEntity = Rhyous.WebFramework.Interfaces.IAddendum;
using EntityService = Rhyous.WebFramework.Services.AddendumService;
using IdType = System.Int64;
using Rhyous.StringLibrary;

namespace Rhyous.WebFramework.WebServices
{
    public class AddendumWebService : IAddendumWebService
    {
        public List<OdataObject<Entity>> GetAll()
        {
            return Service.Get()?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public List<OdataObject<Entity>> GetByIds(List<IdType> ids)
        {
            return Service.Get(ids)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public OdataObject<Entity> Get(string id)
        {
            return Service.Get(id.To<IdType>())?.ToConcrete<Entity>().AsOdata(GetRequestUri());
        }

        public string GetProperty(string id, string property)
        {
            return Service.GetProperty(id.To<IdType>(), property);
        }

        public List<Entity> Post(List<Entity> entities)
        {
            return Service.Add(entities.ToList<IEntity>()).ToConcrete<Entity>().ToList();
        }

        public Entity Patch(string id, Entity entity, List<string> changedProperties)
        {
            return Service.Update(id.To<IdType>(), entity, changedProperties).ToConcrete<Entity>();
        }

        public Entity Put(string id, Entity entity)
        {
            return Service.Replace(id.To<IdType>(), entity).ToConcrete<Entity>();
        }

        public bool Delete(string id)
        {
            return Service.Delete(id.To<IdType>());
        }

        public Uri GetRequestUri()
        {
            return WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri;
        }

        public List<Entity> GetAddenda(string id)
        {
            var entityName = typeof(Entity).Name;
            return Service.Get(x => x.Entity == entityName && x.EntityId == id.ToString())
                                .ToConcrete<Entity>().ToList();
        }

        public Entity GetAddendaByName(string id, string name)
        {
            var entityName = typeof(Entity).Name;
            return Service.Get(x => x.Entity == entityName && x.EntityId == id.ToString())
                                 .OrderByDescending(x => x.CreateDate)
                                 .FirstOrDefault()
                                 .ToConcrete<Entity>();
        }

        #region Injectable Dependency
        internal IServiceCommon<Entity, IEntity, long> Service
        {
            get { return _Service ?? (_Service = new EntityService()); }
            set { _Service = value; }
        } private IServiceCommon<Entity, IEntity, long> _Service;
        #endregion
    }
}
