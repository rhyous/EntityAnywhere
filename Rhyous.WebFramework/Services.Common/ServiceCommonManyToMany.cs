using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public class ServiceCommonManyToMany<T, Tinterface, Tid, TidPrimary, TidSecondary> : ServiceCommon<T, Tinterface, Tid>, IServiceCommonManyToMany<T, Tinterface, Tid, TidPrimary, TidSecondary> 
        where T : class, Tinterface
        where Tinterface : IId<Tid>
    {
        public virtual string PrimaryEntity { get; }
        public virtual string SecondaryEntity { get; }
        public virtual string IdSuffix => "Id";
        
        public virtual List<Tinterface> GetByRelatedEntityId(object id, string entity)
        {
            if (string.IsNullOrWhiteSpace(PrimaryEntity) || string.IsNullOrWhiteSpace(SecondaryEntity))
                throw new InvalidOperationException("Both PrimaryEntity and SecondaryEntity must be assigned a value before this method is called.");
            if (entity != PrimaryEntity && entity != SecondaryEntity)
                throw new ArgumentException($"Valid property names are {PrimaryEntity} or {SecondaryEntity}", "propertyName");
            if (PrimaryEntity.Equals(entity, StringComparison.InvariantCultureIgnoreCase))
                return GetByRelatedPrimaryEntityId((TidPrimary)id, entity);
            if (SecondaryEntity.Equals(entity, StringComparison.InvariantCultureIgnoreCase))
                return GetByRelatedSecondaryEntityId((TidSecondary)id, entity);
            return null;
        }

        public List<Tinterface> GetByRelatedPrimaryEntityId(TidPrimary id, string entity)
        {
            return Repo.GetByExpression((entity + IdSuffix).ToLambda<T, TidPrimary>(id)).ToList();
        }

        public List<Tinterface> GetByRelatedSecondaryEntityId(TidSecondary id, string entity)
        {
            return Repo.GetByExpression((entity + IdSuffix).ToLambda<T, TidSecondary>(id)).ToList();
        }
    }
}