using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.Token;
using IEntity = Rhyous.WebFramework.Interfaces.IToken;
using EntityService = Rhyous.WebFramework.Services.TokenService;
using IdType = System.Int64;
using Rhyous.StringLibrary;

namespace Rhyous.WebFramework.WebServices
{
    public class TokenWebService : ITokenWebService
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

        #region One to Many methods
        public List<OdataObject<Entity>> GetByRelatedEntityId(string id)
        {
            return Service.GetByRelatedEntityId(id.To<IdType>())?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }
        #endregion

        #region Injectable Dependency
        internal IServiceCommonOneToMany<Entity,IEntity, long, long> Service
        {
            get { return _Service ?? (_Service = new EntityService()); }
            set { _Service = value; }
        } private IServiceCommonOneToMany<Entity, IEntity, long, long> _Service;
        #endregion

    }
}
