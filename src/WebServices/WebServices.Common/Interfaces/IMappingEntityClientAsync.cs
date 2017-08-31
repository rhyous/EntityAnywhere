using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public interface IMappingEntityClientAsync<T, Tid, E1Tid, E2Tid> : IMappingEntityClient<T, Tid, E1Tid, E2Tid>, IEntityClient<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where E1Tid : IComparable, IComparable<E1Tid>, IEquatable<E1Tid>
        where E2Tid : IComparable, IComparable<E2Tid>, IEquatable<E2Tid>
    {
        Task<List<OdataObject<T>>> GetByE1IdsAsync(IEnumerable<E1Tid> ids);        
        Task<List<OdataObject<T>>> GetByE2IdsAsync(IEnumerable<E2Tid> ids);
    }
}