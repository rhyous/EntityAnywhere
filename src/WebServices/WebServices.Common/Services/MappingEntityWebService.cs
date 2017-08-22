using LinqKit;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.WebServices
{
    public class MappingEntityWebService<T, Tinterface, Tid, TService, E1Tid, E2Tid>
        : EntityWebService<T, Tinterface, Tid, TService>, IMappingEntityWebService<T, Tid, E1Tid, E2Tid>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>, IMappingEntity<E1Tid, E2Tid>
        where TService : class, IServiceCommon<T, Tinterface, Tid>, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where E1Tid : IComparable, IComparable<E1Tid>, IEquatable<E1Tid>
        where E2Tid : IComparable, IComparable<E2Tid>, IEquatable<E2Tid>
    {
        public List<OdataObject<T>> GetByE1Ids(List<E1Tid> ids)
        {
            var propertyName = typeof(T).GetMappingEntity1Property();
            var lambda = propertyName.ToLambda<Tinterface, E1Tid, bool>(ids);
            return Service.Get(lambda)?.ToConcrete<T, Tinterface>().ToList().AsOdata(RequestUri);
        }

        public List<OdataObject<T>> GetByE2Ids(List<E2Tid> ids)
        {
            var propertyName = typeof(T).GetMappingEntity2Property();
            var lambda = propertyName.ToLambda<Tinterface, E2Tid, bool>(ids);
            return Service.Get(lambda)?.ToConcrete<T, Tinterface>().ToList().AsOdata(RequestUri);
        }
    }
}
