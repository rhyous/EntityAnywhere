using System;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// An interface that all mapping entities must use.
    /// </summary>
    /// <typeparam name="TE1Id">The type of the Id property of the Entity1 type</typeparam>
    /// <typeparam name="TE2Id">The type of the Id property of the Entity2 type.</typeparam>
    public interface IMappingEntity<TE1Id, TE2Id>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
    }
}
