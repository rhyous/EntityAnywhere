using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.UserToUserRole;
using IEntity = Rhyous.WebFramework.Interfaces.IUserToUserRole;
using EntityService = Rhyous.WebFramework.Services.UserToUserRoleService;
using IdType = System.Int64;

namespace Rhyous.WebFramework.WebServices
{
    public class UserToUserRoleWebService : IUserToUserRoleWebService
    {
        public List<OdataObject<Entity>> GetAll()
        {
            return Service.Get()?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public OdataObject<Entity> Get(string id)
        {
            return Service.Get(id.ToInt())?.ToConcrete<Entity>().AsOdata(GetRequestUri());
        }

        public List<OdataObject<Entity>> GetByIds(List<IdType> ids)
        {
            return Service.Get(ids)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
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

        #region Many to Many methods
        public List<OdataObject<Entity>> GetByPrimaryEntityId(string id)
        {
            return Service.GetByRelatedEntityId(id.ToInt(), Service.PrimaryEntity)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public List<OdataObject<Entity>> GetBySecondaryEntityId(string id)
        {
            return Service.GetByRelatedEntityId(id.ToInt(), Service.SecondaryEntity)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }
        #endregion

        #region Injectable Dependency
        internal IServiceCommonManyToMany<Entity, IEntity, long, long, int> Service
        {
            get { return _Service ?? (_Service = new EntityService()); }
            set { _Service = value; }
        } private IServiceCommonManyToMany<Entity, IEntity, long, long, int> _Service;
        #endregion

    }
}
