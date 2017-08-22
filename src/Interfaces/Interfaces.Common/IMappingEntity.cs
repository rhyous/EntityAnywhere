using System;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// These properties should be explicitly implemented and hidden.
    /// </summary>
    /// <typeparam name="E1Tid"></typeparam>
    /// <typeparam name="E2Tid"></typeparam>
    public interface IMappingEntity<E1Tid, E2Tid>
        where E1Tid : IComparable, IComparable<E1Tid>, IEquatable<E1Tid>
        where E2Tid : IComparable, IComparable<E2Tid>, IEquatable<E2Tid>
    {
    }
}
