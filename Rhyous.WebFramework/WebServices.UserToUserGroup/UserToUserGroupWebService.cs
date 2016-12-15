using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using Entity = Rhyous.WebFramework.Services.UserToUserGroup;
using IEntity = Rhyous.WebFramework.Interfaces.IUserToUserGroup;
using EntityService = Rhyous.WebFramework.Services.UserToUserGroupService;
using IdType = System.Int64;
using Rhyous.StringLibrary;

namespace Rhyous.WebFramework.WebServices
{
    public class UserToUserGroupWebService : IUserToUserGroupWebService
    {
        public List<OdataObject<Entity>> GetAll()
        {
            return Service.Get()?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public OdataObject<Entity> Get(string id)
        {
            return Service.Get(id.To<IdType>())?.ToConcrete<Entity>().AsOdata(GetRequestUri());
        }

        public List<OdataObject<Entity>> GetByIds(List<IdType> ids)
        {
            return Service.Get(ids)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
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

        #region Many to Many methods
        public List<OdataObject<Entity>> GetByPrimaryEntityId(string id)
        {
            return Service.GetByRelatedEntityId(id.To<IdType>(), Service.PrimaryEntity)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
        }

        public List<OdataObject<Entity>> GetBySecondaryEntityId(string id)
        {
            return Service.GetByRelatedEntityId(id.To<IdType>(), Service.SecondaryEntity)?.ToConcrete<Entity>().ToList().AsOdata(GetRequestUri());
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
