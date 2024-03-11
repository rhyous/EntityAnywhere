using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityInfoMapping<TEntity, TE1Id, TE2Id>
        where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
        where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
    {
        Expression<Func<TEntity, TE1Id>> E1PropertyExpression { get; }
        Func<TEntity, TE1Id> E1PropertyExpressionMethod { get; }
        PropertyInfo E1PropertyInfo { get; }
        Expression<Func<TEntity, TE2Id>> E2PropertyExpression { get; }
        Func<TEntity, TE2Id> E2PropertyExpressionMethod { get; }
        PropertyInfo E2PropertyInfo { get; }
    }
}