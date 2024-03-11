using System;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityInfoAltKey<TEntity, TAltKey> : IEntityInfo<TEntity>
    {
        string AlternateKeyProperty { get; }

        Expression<Func<TEntity, TAltKey>> PropertyExpression { get; }
        Func<TEntity, TAltKey> PropertyExpressionMethod { get; }
    }
}