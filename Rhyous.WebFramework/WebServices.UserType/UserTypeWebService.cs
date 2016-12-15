using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.UserType;
using IEntity = Rhyous.WebFramework.Interfaces.IUserType;
using EntityService = Rhyous.WebFramework.Services.UserTypeService;
using IdType = System.Int32;
using Rhyous.StringLibrary;

namespace Rhyous.WebFramework.WebServices
{
    public class UserTypeWebService : IUserTypeWebService
    {
        public List<OdataObject<Entity>> GetAll()
        {
            return Service.Get()?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public List<OdataObject<Entity>> GetByIds(List<IdType> ids)
        {
            return Service.Get(ids)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public OdataObject<Entity> Get(string idOrName)
        {
            if (string.IsNullOrWhiteSpace(idOrName))
                return null;
            if (idOrName.Any(c=>!char.IsDigit(c)))
                return Service.Get(idOrName)?.ToConcrete<Entity>().AsOdata(GetRequestUri());
            return Service.Get(idOrName.ToInt())?.ToConcrete<Entity>().AsOdata(GetRequestUri());
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

        #region Injectable Dependency
        internal ISearchableServiceCommon<Entity,IEntity,int> Service
        {
            get { return _Service ?? (_Service = new EntityService()); }
            set { _Service = value; }
        } private ISearchableServiceCommon<Entity, IEntity, int> _Service;
        #endregion
    }
}
