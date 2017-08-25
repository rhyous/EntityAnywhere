using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Clients
{
    public interface IMappingEntityClient<T, Tid, E1Tid, E2Tid> : IMappingEntityWebService<T, Tid, E1Tid, E2Tid>, IEntityClient<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where E1Tid : IComparable, IComparable<E1Tid>, IEquatable<E1Tid>
        where E2Tid : IComparable, IComparable<E2Tid>, IEquatable<E2Tid>
    {
        string Entity1 { get; }
        string Entity1Pluralized { get; }
        string Entity1Property { get; }

        string Entity2 { get; }
        string Entity2Pluralized { get; }
        string Entity2Property { get; }

        List<OdataObject<T>> GetByE1Ids(IEnumerable<E1Tid> ids);        
        List<OdataObject<T>> GetByE2Ids(IEnumerable<E2Tid> ids);
    }
}