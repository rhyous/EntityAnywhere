using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public class MappingEntityWebService<TEntity, TInterface, TId, TService, TE1Id, TE2Id>
        : EntityWebService<TEntity, TInterface, TId, TService>, IMappingEntityWebService<TEntity, TId, TE1Id, TE2Id>
        where TEntity : class, TInterface, new()
        where TInterface : IEntity<TId>, IMappingEntity<TE1Id, TE2Id>
        where TService : class, IServiceCommon<TEntity, TInterface, TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        /// <inheritdoc />
        public List<OdataObject<TEntity>> GetByE1Ids(List<TE1Id> ids)
        {
            var propertyName = typeof(TEntity).GetMappedEntity1Property();
            var lambda = propertyName.ToLambda<TInterface, TE1Id>(ids);
            return Service.Get(lambda)?.ToConcrete<TEntity, TInterface>().ToList().AsOdata(RequestUri);
        }

        /// <inheritdoc />
        public List<OdataObject<TEntity>> GetByE2Ids(List<TE2Id> ids)
        {
            var propertyName = typeof(TEntity).GetMappedEntity2Property();
            var lambda = propertyName.ToLambda<TInterface, TE2Id>(ids);
            return Service.Get(lambda)?.ToConcrete<TEntity, TInterface>().ToList().AsOdata(RequestUri);
        }
    }
}
