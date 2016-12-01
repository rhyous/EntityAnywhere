using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public class ServiceCommonManyToMany<T, Tinterface> : ServiceCommon<T, Tinterface>, IServiceCommonManyToMany<T, Tinterface> 
        where T : class
    {
        public virtual string PrimaryEntity { get; }
        public virtual string SecondaryEntity { get; }
        public virtual string IdSuffix => "Id";

        public virtual List<Tinterface> GetByRelatedEntityId(int id, string entity)
        {
            if (string.IsNullOrWhiteSpace(PrimaryEntity) || string.IsNullOrWhiteSpace(SecondaryEntity))
                throw new InvalidOperationException("Both PrimaryEntity and SecondaryEntity must be assigned a value before this method is called.");
            if (entity != PrimaryEntity && entity != SecondaryEntity)
                throw new ArgumentException($"Valid property names are {PrimaryEntity} or {SecondaryEntity}", "propertyName");
            var entityIdColumn = entity + IdSuffix;
            return Repo.GetByExpression(entityIdColumn.ToLambda<T, int>(id)).ToList();
        }
    }
}