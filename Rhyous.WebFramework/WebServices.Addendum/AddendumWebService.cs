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
            return Service.Get(id.ToInt())?.ToConcrete<Entity>().AsOdata(GetRequestUri());
        }

        public string GetProperty(string id, string property)
        {
            return Service.GetProperty(id.ToInt(), property);
        }

        public List<Entity> Post(List<Entity> entities)
        {
            return Service.Add(entities.ToList<IEntity>()).ToConcrete<Entity>().ToList();
        }

        public Entity Patch(string id, Entity entity, List<string> changedProperties)
        {
            return Service.Update(id.ToInt(), entity, changedProperties).ToConcrete<Entity>();
        }

        public Entity Put(string id, Entity entity)
        {
            return Service.Replace(id.ToInt(), entity).ToConcrete<Entity>();
        }

        public bool Delete(string id)
        {
            return Service.Delete(id.ToInt());
        }

        public Uri GetRequestUri()
        {
            return WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri;
        }

        #region Injectable Dependency
        internal IServiceCommonOneToMany<Entity, IEntity, long, string> Service
        {
            get { return _Service ?? (_Service = new EntityService()); }
            set { _Service = value; }
        } private IServiceCommonOneToMany<Entity, IEntity, long, string> _Service;
        #endregion
    }
}
